use crate::utils::basic_utils::{write_request, get_response, judge_response, unwrap_str};
use crate::utils::mail_utils::{LoginInfo, MailInfo, Mail};
use crate::utils::mail_utils::{authenticate, send_mail, send_mail_extern};


mod utils {
    
    pub mod basic_utils {
        use std::str;
        use std::net::TcpStream;
        use std::io::{Read, Write};
        use base64::encode;
        use std::ffi::CStr;
        use libc::c_char;
        
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
            let _ = socket.write(&(message.as_bytes()));
            let _ = socket.write("\r\n".as_bytes());
        }

        pub fn unwrap_str(s: *const c_char) -> String {
            let s = unsafe { CStr::from_ptr(s) };
            String::from(s.to_str().unwrap())
        }
    }
    
    pub mod mail_utils {
        use std::net::TcpStream;
        use std::io::Write;
        use libc::c_char;
        use chrono::prelude::*;
        use crate::utils::basic_utils::*;
        
        static CRLF: &str = "\r\n";

        #[repr(C)]
        pub struct LoginInfo {    
            pub account: *const c_char,
            pub passwd: *const c_char,
            pub site: *const c_char,
        }

        pub struct MailInfo {
            pub from: *const c_char,
            pub to: *const c_char,
            pub cc: *const c_char,
            pub subject: *const c_char,
            pub body: *const c_char
        }
        
        pub struct Mail {
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

        pub fn authenticate(login_info: &LoginInfo) -> (bool, TcpStream) {
            let account_str = unwrap_str(login_info.account);
            let passwd_str = unwrap_str(login_info.passwd);
            let site_str = unwrap_str(login_info.site);
            
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
                    return (false, stream);
                }
            }
            (true, stream)
        }

        pub fn send_mail(socket: &mut TcpStream, mail: Mail) {
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

        pub fn send_mail_extern(socket: &mut TcpStream, mail: Mail) {
            write_request(socket, &format!("MAIL FROM: <{}>", mail.from));
            get_response(socket);
            
            write_request(socket, &format!("RCPT TO: <{}>", mail.to));
            get_response(socket);
            
            write_request(socket, "DATA");
            get_response(socket);
            
            send_body(socket, &mail);
            // get_response(socket);
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
    }
}

#[no_mangle]
pub extern "C" fn validate_account(login_info: LoginInfo) -> bool {
    return authenticate(&login_info).0;
}

#[no_mangle]
pub extern "C" fn login_send_mail(login_info: LoginInfo, mail_info: MailInfo) -> i32 {
    let (result, mut stream) = authenticate(&login_info);

    if !result {
        return 400;
    }

    let mail = Mail::new(unwrap_str(mail_info.from), unwrap_str(mail_info.to), 
    unwrap_str(mail_info.cc), unwrap_str(mail_info.subject), unwrap_str(mail_info.body));

    send_mail(&mut stream, mail);
    get_response(&mut stream);

    write_request(&mut stream, "QUIT");
    get_response(&mut stream);

    200
}

#[no_mangle]
pub extern "C" fn login_send_mail_extern(login_info: LoginInfo, mail_info: MailInfo) -> i32 {
    let (result, mut stream) = authenticate(&login_info);

    if !result {
        return 400;
    }

    let mail = Mail::new(unwrap_str(mail_info.from), unwrap_str(mail_info.to), 
    unwrap_str(mail_info.cc), unwrap_str(mail_info.subject), unwrap_str(mail_info.body));

    let mut responses: Vec<String> = Vec::new();

    send_mail_extern(&mut stream, mail);
    responses.push(get_response(&mut stream));

    write_request(&mut stream, "QUIT");
    get_response(&mut stream);

    for response in responses {
        if !judge_response(&response) {
            return 401;
        }
    }

    200
}
