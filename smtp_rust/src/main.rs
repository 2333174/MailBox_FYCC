use std::net::TcpStream;
use std::io::{Read, Write};
use std::str;
use base64::encode;
use chrono::prelude::*;

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

fn get_response(socket: &mut TcpStream) {

    let mut buf: [u8; 1024] = [0; 1024];

    let _ = socket.read(&mut buf);
    let response = str::from_utf8(&buf).unwrap();
    println!("{}", response);
}

fn write_request(socket: &mut TcpStream, message: &str) {
    let message = format!("{}{}", message, "\r\n");
    let _ = socket.write(&(message.as_bytes()));
}

fn write_request_b64(socket: &mut TcpStream, message: &str) {
    let message = encode(message.as_bytes());
    println!("{}", message);
    let _ = socket.write(&(message.as_bytes()));
    let _ = socket.write("\r\n".as_bytes());
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

fn main() -> std::io::Result<()> {

    let login = "ale_li_pona@163.com";
    let password = "ybg19970203ybg";

    let mut stream = TcpStream::connect("smtp.163.com:25")?;
    get_response(&mut stream);

    write_request(&mut stream, "HELO ale_li_pona");
    get_response(&mut stream);

    write_request(&mut stream, "AUTH LOGIN");
    get_response(&mut stream);

    write_request_b64(&mut stream, &login);
    get_response(&mut stream);

    write_request_b64(&mut stream, &password);
    get_response(&mut stream);

    let mail = Mail::new(String::from(login), "ale_li_pona@163.com".to_string(), 
        String::from(login), "Hello Rust".to_string(), "nothing to say".to_string());

    send_mail(&mut stream, mail);
    get_response(&mut stream);

    write_request(&mut stream, "QUIT");
    get_response(&mut stream);

    Ok(())
} 
