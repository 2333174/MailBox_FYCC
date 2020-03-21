from email.mime.text import MIMEText
from email.mime.multipart import MIMEMultipart
import sys
from email.utils import parseaddr, formataddr
from email import encoders
from email.header import Header

def _format_addr(s):
    name, addr = parseaddr(s)
    return formataddr((Header(name, 'utf-8').encode(), addr))

def getMimeWithText(content,from_add,to_add,subject):
    '''
    当没有附件时，调用此函数
    :param content: 邮件的文本内容
    :param from_add: 发出地址
    :param to_add: 接受地址
    :param subject: 主题
    :return: 返回的Mime包装的邮件字符串
    '''
    msg = MIMEText(content, 'plain', 'utf-8')
    msg['From'] = _format_addr('%s <%s>' % (from_add, from_add))
    msg['To'] = _format_addr('%s <%s>' % (to_add, to_add))
    msg['Subject'] = Header('%s' % subject, 'utf-8').encode()
    return msg.as_string()

def getMimeWithFile(content,from_add,to_add,subject,pathList):
    '''
    当有附件时，调用此函数
    :param content: 邮件的文本内容
    :param from_add: 发出地址
    :param to_add: 接受地址
    :param subject: 主题
    :param pathList: 路径数组
    :return: Mime封装的邮件字符串
    '''
    pass
    msg = MIMEMultipart()
    msg['From'] = _format_addr('%s <%s>' % (from_add, from_add))
    msg['To'] = _format_addr('%s <%s>' % (to_add, to_add))
    msg['Subject'] = Header('%s' % subject, 'utf-8').encode()
    msg.attach(MIMEText(content, 'plain', 'utf-8'))
    attachFile(msg,pathList)
    return msg.as_string()




def attachFile(msg,pathList):
    for each in pathList:
        fileName = each.split('/')[-1]
        att1 = MIMEText(open(each, 'rb').read(), 'base64', 'utf-8')
        att1["Content-Type"] = 'application/octet-stream'
        att1["Content-Disposition"] = 'attachment; filename="'+fileName+'"'
        msg.attach(att1)


content = sys.argv[1]
from_add = sys.argv[2]
to_add =sys.argv[3]
subject = sys.argv[4]
if len(sys.argv)<=5:
    print(getMimeWithText(content,from_add,to_add,subject))
else:
    pathList = sys.argv[5:]
    print(getMimeWithFile(content,from_add,to_add,subject,pathList))

# myTest
# from_add = "alertdoll@163.com"
# to_add = "935802216@qq.com"
# subject ="123"
# content="456"
# pathList =['D:/吕若凡/github.txt','D:/吕若凡/大三下/简历/1.jpg']
# print(getMimeWithFile(content,from_add,to_add,subject,pathList))