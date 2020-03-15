use std::net::TcpStream;
use crate::utils::mail_utils::{unwrap_str, LoginInfo};
use crate::utils::basic_utils::*;
use crate::utils::mail_utils::*;
use std::ffi::{CStr, CString};
use libc::c_char;

static mut STRING_POINTER: *mut c_char = 0 as *mut c_char;

mod utils {
    
    pub mod basic_utils {
        use regex::Regex;
        use std::net::TcpStream;
        use std::str;
        use std::io::{Read, Write};
        use std::ffi::{CStr, CString};
        use std::os::raw::c_char;
        
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
        
        pub fn authenticate(login_info: LoginInfo) -> (bool, TcpStream) {
            
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
            
            for response in responses {
                if !judge_response(&response) {
                    return (false, stream);
                }
            }
            (true, stream)
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
pub extern "C" fn pull_a_mail(login_info: LoginInfo, index: usize) -> *mut c_char {
    
    let (result, mut stream) = authenticate(login_info);
    
    if !result {
        return CString::new("400").unwrap().into_raw();
    }
    
    let mail_str = get_a_mail(&mut stream, index);
    
    let mail_cstr = CString::new(mail_str).unwrap();
    
    mail_cstr.into_raw()
}


#[repr(C)]
pub struct ResultStruct {    
    pub status: i32,
    pub mail_string: *mut c_char,
}

fn store_string_on_heap(string_to_store: &String) -> *mut c_char {
    //create a new raw pointer
    let pntr = CString::new(String::from(string_to_store)).unwrap().into_raw();
    //store it in our static variable (REQUIRES UNSAFE)
    unsafe {
        STRING_POINTER = pntr;
    }
    //return the c_char
    return pntr;
}

// static mut MAIL_STR: &'static str = "" as &'static str;
static mut MY_STRING: String = String::new();

#[no_mangle]
pub extern fn get_simple_email(login_info: LoginInfo, index: usize) -> ResultStruct {
    
    let (result, mut stream) = authenticate(login_info);
    
    unsafe {

        MY_STRING = String::from("WTF");

        ResultStruct {
            status: 400,
            mail_string: store_string_on_heap(&MY_STRING),
        }

        // if result {
        //     MY_STRING.clear();
        //     MY_STRING.push_str(&get_a_mail(&mut stream, index));
            
        //     ResultStruct {
        //         status: 200,
        //         mail_string: store_string_on_heap(&MY_STRING),
        //     }
        // } else {
        //     MY_STRING.clear();
            
        //     ResultStruct {
        //         status: 400,
        //         mail_string: store_string_on_heap(&MY_STRING),
        //     }
        // }
    }
}

#[no_mangle]
pub extern fn free_string() {
    unsafe {
        let _ = CString::from_raw(STRING_POINTER);
        STRING_POINTER = 0 as *mut c_char;
    }
}
