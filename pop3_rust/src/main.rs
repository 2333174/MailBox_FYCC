use std::net::TcpStream;
use std::io::{Read, Write};
use std::str;
use regex::Regex;


fn is_final_end(s: &str) -> bool {
    let re = Regex::new(r"\r\n\.\r\n").unwrap();
    re.is_match(s)
}

fn has_char_crlf(s: &str) -> bool {
    let re = Regex::new(r"[\r\n]").unwrap();
    re.is_match(s)
}

fn get_response(socket: &mut TcpStream) -> String {
    
    let mut buf: [u8; 1024] = [0; 1024];
    
    let _ = socket.read(&mut buf);
    let response = str::from_utf8(&buf).unwrap();
    let response = response.to_string();
    
    // if response[response.len()-2..] == "\r\n".to_string() {
    //     response = response[..response.len()-2].to_string();
    // }
    // if has_char_crlf(&response[response.len()-1..]) {
    //     response = response[..response.len()-1].to_string();
    // }

    // println!("Length of response is {}", response.len());
    
    response
}

fn write_request(socket: &mut TcpStream, message: &str) {
    let message = format!("{}{}", message, "\r\n");
    let _ = socket.write(&(message.as_bytes()));
}

fn authenticate(socket: &mut TcpStream, user: &str, pwd: &str) -> i32 {
    write_request(socket, &format!("{} {}", "USER", user));
    get_response(socket);
    write_request(socket, &format!("{} {}", "PASS", pwd));
    let resp = get_response(socket);
    
    let tokens:Vec<&str> = resp.split(" ").collect();
    
    tokens[1].parse().unwrap()
}

fn get_multiresponses(socket: &mut TcpStream) -> String {
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

fn list_mails(socket: &mut TcpStream) {
    write_request(socket, "LIST");
    
    println!("{}", get_multiresponses(socket)); 
}

fn get_a_mail(socket: &mut TcpStream, index: usize) -> String {
    write_request(socket, &format!("RETR {}", index));
    get_multiresponses(socket)
}

fn main() -> std::io::Result<()> {
    
    let login = "ale_li_pona@163.com";
    let password = "ybg19970203ybg";
    
    let mut index = 0;
    
    let mut stream = TcpStream::connect("pop.163.com:110")?;
    let response = get_response(&mut stream);
    println!("{}", response[100..].to_string());
    
    index = authenticate(&mut stream, login, password);
    println!("There are {} mails.", index);
    
    list_mails(&mut stream);
    
    let mail_str = get_a_mail(&mut stream, 445);
    println!("{}", mail_str);

    write_request(&mut stream, "QUIT");
    get_response(&mut stream);
    
    Ok(())
}
