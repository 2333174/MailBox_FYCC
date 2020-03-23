use std::net::TcpStream;
use crate::utils::mail_utils::{unwrap_str, LoginInfo};
use crate::utils::basic_utils::*;
use crate::utils::mail_utils::*;
use std::ffi::{CStr, CString};
use libc::c_char;
use std::mem;
use std::fs::File;
use std::io;
use std::io::prelude::*;
use std::io::BufReader;
use std::fs;

static mut STRING_POINTER: *mut c_char = 0 as *mut c_char;

mod utils {
    
    pub mod basic_utils {
        use regex::Regex;
        use std::net::TcpStream;
        use std::str;
        use std::io::{Read, Write};
        use std::ffi::{CStr, CString};
        use std::os::raw::c_char;
        use std::fs;
        
        pub fn get_response(socket: &mut TcpStream) -> String{
            let mut buf: [u8; 1024] = [0; 1024];
            
            let _ = socket.read(&mut buf);
            let response = str::from_utf8(&buf).unwrap();
            // println!("{}", response);
            String::from(response)
        }
        
        pub fn judge_response(response: &str) -> bool {
            let result = match &response[..3] {
                "+OK" => true,
                _ => false
            };
            
            result
        }
        
        pub fn is_final_end(s: &str) -> bool {
            let re = Regex::new(r"\r\n\.\r\n").unwrap();
            re.is_match(s)
        }
        
        pub fn has_char_crlf(s: &str) -> bool {
            let re = Regex::new(r"[\r\n]").unwrap();
            re.is_match(s)
        }
        
        pub fn write_request(socket: &mut TcpStream, message: &str) {
            let message = format!("{}{}", message, "\r\n");
            let _ = socket.write(&(message.as_bytes()));
        }
        
        pub fn path_exists(path: &str) -> bool {
            fs::metadata(path).is_ok()
        }
    }
    
    pub mod mail_utils {
        use std::net::TcpStream;
        use std::str;
        use libc::c_char;
        use std::ffi::CStr;
        use crate::utils::basic_utils::*;
        
        static CRLF: &str = "\r\n";
        
        #[repr(C)]
        pub struct LoginInfo {    
            pub account: *const c_char,
            pub passwd: *const c_char,
            pub site: *const c_char,
        }
        
        pub fn unwrap_str(s: *const c_char) -> String {
            let s = unsafe { CStr::from_ptr(s) };
            String::from(s.to_str().unwrap())
        }
        
        pub fn get_multiresponses(socket: &mut TcpStream) -> String {
            let mut resp = String::new();
            
            loop {
                let resp_part = get_response(socket);
                
                resp = format!("{}{}", resp, resp_part);
                
                if is_final_end(&resp_part) {
                    break;
                }
            }
            
            resp
        }
        
        pub fn authenticate(login_info: LoginInfo) -> (bool, String, TcpStream) {
            
            let account_str = unwrap_str(login_info.account);
            let passwd_str = unwrap_str(login_info.passwd);
            let site_str = unwrap_str(login_info.site);
            
            let mut responses: Vec<String> = Vec::new();
            
            let mut stream = TcpStream::connect(&site_str).unwrap();
            responses.push(get_response(&mut stream));
            
            write_request(&mut stream, &format!("{} {}", "USER", account_str));
            responses.push(get_response(&mut stream));
            write_request(&mut stream, &format!("{} {}", "PASS", passwd_str));
            responses.push(get_response(&mut stream));
            
            for response in &responses {
                if !judge_response(&response) {
                    return (false, String::from("-1"), stream);
                }
            }

            let mut iter = responses.get(2).unwrap().split_whitespace();

            let _ = iter.next();
            let num_mails = iter.next().unwrap();

            (true, String::from(num_mails), stream)
        }
        
        pub fn list_mails(socket: &mut TcpStream) -> String {
            write_request(socket, "LIST");
            
            // println!("{}", get_multiresponses(socket));
            get_multiresponses(socket) 
        }
        
        pub fn get_a_mail(socket: &mut TcpStream, index: usize) -> String {
            println!("RETR {}", index);
            write_request(socket, &format!("RETR {}", index));
            get_multiresponses(socket)
        }
        
    }
}

#[no_mangle]
pub extern "C" fn validate_account(login_info: LoginInfo) -> bool {
    authenticate(login_info).0
}

#[no_mangle]
pub extern "C" fn get_num_mails(login_info: LoginInfo) -> i32 {
    let (result, num_mails_Str, mut stream) = authenticate(login_info);

    let num_mails = num_mails_Str.parse::<i32>().unwrap();

    num_mails
}

#[no_mangle]
pub extern "C" fn pull_save_mail(login_info: LoginInfo, index: usize) -> i32 {
    let account_str = unwrap_str(login_info.account);

    let (result, _,  mut stream) = authenticate(login_info);
    
    if !result {
        return 400;
    }
    
    let mail_str = get_a_mail(&mut stream, index);    

    if !path_exists(&account_str) {
        fs::create_dir(&account_str).unwrap();
    }

    let path: &str = &format!("{}/{}-{}.mail.tmp", account_str, account_str, index);
    let mut output: File = File::create(path).unwrap();
    write!(output, "{}", mail_str).unwrap();
    
    200
}