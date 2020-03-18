from email.mime.text import MIMEText
import sys
from email.utils import parseaddr, formataddr
from email import encoders
from email.header import Header

def _format_addr(s):
    name, addr = parseaddr(s)
    return formataddr((Header(name, 'utf-8').encode(), addr))

def main(content,from_add,to_add,subject):
    msg = MIMEText(content, 'plain', 'utf-8')
    msg['From'] = _format_addr('%s <%s>' % (from_add, from_add))
    msg['To'] = _format_addr('%s <%s>' % (to_add, to_add))
    msg['Subject'] = Header('%s' % subject, 'utf-8').encode()
    return msg.as_string()

content = sys.argv[1]
from_add = sys.argv[2]
to_add =sys.argv[3]
subject = sys.argv[4]

print(main(content,from_add,to_add,subject))