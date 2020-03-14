use std::net::TcpStream;
use std::str;
use crate::utils::basic_utils::{write_request, get_response, write_request_b64, judge_response};
use crate::utils::unwrap_str;
use libc::c_char;

mod utils {
    use libc::c_char;
    use std::ffi::CStr;

    pub mod basic_utils {
        use std::str;
        use std::net::TcpStream;
        use std::io::{Read, Write};
        use base64::encode;
        
        pub fn get_response(socket: &mut TcpStream) -> String{
            let mut buf: [u8; 1024] = [0; 1024];
            
            let _ = socket.read(&mut buf);
            let response = str::from_utf8(&buf).unwrap();
            println!("{}", response);
            String::from(response)
        }
        
        pub fn judge_response(response: &str) -> bool {
            
            let result = match &response[..1] {
                "2" => true,
                "3" => true,
                _ => false
            };
            
            result
        }
        
        pub fn write_request(socket: &mut TcpStream, message: &str) {
            let message = format!("{}{}", message, "\r\n");
            let _ = socket.write(&(message.as_bytes()));
        }
        
        pub fn write_request_b64(socket: &mut TcpStream, message: &str) {
            let message = encode(message.as_bytes());
            println!("{}", message);
            let _ = socket.write(&(message.as_bytes()));
            let _ = socket.write("\r\n".as_bytes());
        }
    }
    
    pub mod mail_utils {
        use std::net::TcpStream;
        use std::io::Write;
        use chrono::prelude::*;
        use crate::utils::basic_utils::{write_request, get_response};

        static CRLF: &str = "\r\n";

        struct Mail {
            from: String,
            to: String,
            cc: String,
            subject: String,
            body: String
        }
        
        impl Mail {
            pub fn new(from: String, to: String, cc: String, subject: String, body: String) -> Mail {
                Mail {
                    from: from,
                    to: to,
                    cc: cc,
                    subject: subject,
                    body: body
                }
            }
        }

        fn send_head(socket: &mut TcpStream, mail: &Mail) {
            let now = Utc::now();
            
            write_request(socket, &format!("Date: {}", now));
            write_request(socket, &format!("From: a {}", mail.from));
            write_request(socket, &format!("To: b {}", mail.to));
            write_request(socket, &format!("Cc: c {}", mail.cc));
            write_request(socket, &format!("Subject: {}", mail.subject));
        }
        
        fn send_body(socket: &mut TcpStream, mail: &Mail) {
            let _ = socket.write(mail.body.as_bytes());
            let _ = socket.write(format!("{}.{}", CRLF, CRLF).as_bytes());
        }
        
        fn send_mail(socket: &mut TcpStream, mail: Mail) {
            write_request(socket, &format!("MAIL FROM: <{}>", mail.from));
            get_response(socket);
            
            write_request(socket, &format!("RCPT TO: <{}>", mail.to));
            get_response(socket);
            
            write_request(socket, "DATA");
            get_response(socket);
            
            send_head(socket, &mail);
            // get_response(socket);
            
            write_request(socket, "");
            
            send_body(socket, &mail);
            // get_response(socket);
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
    let tokens:Vec<&str>= account_str.split("@").collect();
    
    let mut stream = TcpStream::connect(&site_str).unwrap();
    responses.push(get_response(&mut stream));
    
    write_request(&mut stream, &format!("HELO {}", tokens[0]));
    responses.push(get_response(&mut stream));
    
    write_request(&mut stream, "AUTH LOGIN");
    responses.push(get_response(&mut stream));
    
    write_request_b64(&mut stream, &account_str);
    responses.push(get_response(&mut stream));
    
    write_request_b64(&mut stream, &passwd_str);
    responses.push(get_response(&mut stream));
    
    for response in responses {
        if !judge_response(&response) {
            return false;
        }
    }
    true
}

