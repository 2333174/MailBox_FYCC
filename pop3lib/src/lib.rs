use std::net::TcpStream;
use libc::c_char;
use crate::utils::unwrap_str;
use crate::utils::basic_utils::*;
use crate::utils::mail_utils::*;



mod utils {
    use libc::c_char;
    use std::ffi::CStr;
    
    pub mod basic_utils {
        use regex::Regex;
        use std::net::TcpStream;
        use std::str;
        use std::io::{Read, Write};
        
        pub fn get_response(socket: &mut TcpStream) -> String{
            let mut buf: [u8; 1024] = [0; 1024];
            
            let _ = socket.read(&mut buf);
            let response = str::from_utf8(&buf).unwrap();
            println!("{}", response);
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
        use crate::utils::basic_utils::*;

        static CRLF: &str = "\r\n";
        
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
        
        pub fn authenticate(socket: &mut TcpStream, user: &str, pwd: &str) {
            write_request(socket, &format!("{} {}", "USER", user));
            get_response(socket);
            write_request(socket, &format!("{} {}", "PASS", pwd));
            // let resp = get_response(socket);
            // let tokens:Vec<&str> = resp.split(" ").collect();
            // tokens[1].parse().unwrap()
        }
        
        pub fn list_mails(socket: &mut TcpStream) {
            write_request(socket, "LIST");
            
            println!("{}", get_multiresponses(socket)); 
        }
        
        pub fn get_a_mail(socket: &mut TcpStream, index: usize) -> String {
            write_request(socket, &format!("RETR {}", index));
            get_multiresponses(socket)
        }
        
    }
    
    pub fn unwrap_str(s: *const c_char) -> String {
        let s = unsafe { CStr::from_ptr(s) };
        String::from(s.to_str().unwrap())
    }
    
}

#[no_mangle]
pub extern "C" fn validate_account(account: *const c_char, passwd: *const c_char, site: *const c_char) -> bool {
    
    let account_str = unwrap_str(account);
    let passwd_str = unwrap_str(passwd);
    let site_str = unwrap_str(site);
    
    let mut responses: Vec<String> = Vec::new();
    
    let mut stream = TcpStream::connect(&site_str).unwrap();
    responses.push(get_response(&mut stream));
    
    authenticate(&mut stream, &account_str, &passwd_str);
    responses.push(get_response(&mut stream));
    
    for response in responses {
        if !judge_response(&response) {
            return false;
        }
    }
    true
}
