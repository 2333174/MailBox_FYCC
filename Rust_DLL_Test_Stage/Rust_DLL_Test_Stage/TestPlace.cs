using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;
using System.Runtime.InteropServices;

namespace Rust_DLL_Test_Stage
{
    class TestPlace
    {
        static String mailstr = "MIME-Version: 1.0\r\nDate: Mon, 24 Feb 2020 23:40:22 +0800\r\nFrom: GitHub <alertdoll@163.com>\r\nSubject: [GitHub] @woods321 has invited you to join the @20192021855-DCAN\r\nThread-Topic: [GitHub] @woods321 has invited you to join the\r\n @20192021855-DCAN organization\r\nTo: Johnson <ale_li_pona@163.com>\r\nContent-Transfer-Encoding: quoted-printable\r\nContent-Type: text/html; charset=\"utf-8\"\r\n\r\n<!DOCTYPE html PUBLIC \"-//W3C//DTD HTML 4.01 Transitional//EN\" \"http://www.=\r\nw3.org/TR/html4/loose.dtd\">\r\n<html lang=3D\"en\">\r\n<head>\r\n<meta http-equiv=3D\"Content-Type\" content=3D\"text/html; charset=3Dutf-8\">\r\n<title>[GitHub] @woods321 has invited you to join the @20192021855-DCAN org=\r\nanization</title>\r\n</head>\r\n<body bgcolor=3D\"#fafafa\" topmargin=3D\"0\" leftmargin=3D\"0\" marginheight=3D\"=\r\n0\" marginwidth=3D\"0\" style=3D\"-ms-text-size-adjust: 100%; -webkit-font-smoo=\r\nthing: antialiased; -webkit-text-size-adjust: 100%; background: #fafafa; co=\r\nlor: #333333; font-family: 'Helvetica Neue', Helvetica, Arial, sans-serif; =\r\nfont-size: 14px; font-weight: normal; line-height: 20px; margin: 0; min-wid=\r\nth: 100%; padding: 0; text-align: center; width: 100% !important\">\r\n<style type=3D\"text/css\">\r\nbody {\r\nwidth: 100% !important; min-width: 100%; -webkit-font-smoothing: antialiase=\r\nd; -webkit-text-size-adjust: 100%; -ms-text-size-adjust: 100%; margin: 0; p=\r\nadding: 0; background: #fafafa;\r\n}\r\n.ExternalClass {\r\nwidth: 100%;\r\n}\r\n.ExternalClass {\r\nline-height: 100%;\r\n}\r\n#backgroundTable {\r\nmargin: 0; padding: 0; width: 100% !important; line-height: 100% !important=\r\n;\r\n}\r\nimg {\r\noutline: none; text-decoration: none; -ms-interpolation-mode: bicubic; widt=\r\nh: auto; max-width: 100%;\r\n}\r\nbody {\r\ncolor: #333333; font-family: 'Helvetica Neue', Helvetica, Arial, sans-serif=\r\n; font-weight: normal; padding: 0; margin: 0; text-align: center; line-heig=\r\nht: 1.3;\r\n}\r\nbody {\r\nfont-size: 14px; line-height: 20px;\r\n}\r\na:hover {\r\ncolor: #4183C4;\r\n}\r\na:active {\r\ncolor: #4183C4;\r\n}\r\na:visited {\r\ncolor: #4183C4;\r\n}\r\nh1 a:active {\r\ncolor: #4183C4;\r\n}\r\nh2 a:active {\r\ncolor: #4183C4;\r\n}\r\nh3 a:active {\r\ncolor: #4183C4;\r\n}\r\nh4 a:active {\r\ncolor: #4183C4;\r\n}\r\nh5 a:active {\r\ncolor: #4183C4;\r\n}\r\nh6 a:active {\r\ncolor: #4183C4;\r\n}\r\nh1 a:visited {\r\ncolor: #4183C4;\r\n}\r\nh2 a:visited {\r\ncolor: #4183C4;\r\n}\r\nh3 a:visited {\r\ncolor: #4183C4;\r\n}\r\nh4 a:visited {\r\ncolor: #4183C4;\r\n}\r\nh5 a:visited {\r\ncolor: #4183C4;\r\n}\r\nh6 a:visited {\r\ncolor: #4183C4;\r\n}\r\nbody.outlook p {\r\ndisplay: inline !important;\r\n}\r\n@media only screen and (max-width: 600px) {\r\n  table[class=3D\"body\"] h1.primary-heading {\r\n    font-size: 18px !important;\r\n  }\r\n  table[class=3D\"body\"] div.panel p {\r\n    line-height: 17px !important;\r\n  }\r\n  table[class=3D\"body\"] img {\r\n    width: auto !important; height: auto !important;\r\n  }\r\n  table[class=3D\"body\"] img.avatar {\r\n    width: 40px !important; height: 40px !important;\r\n  }\r\n  table[class=3D\"body\"] img.content-header-octicon {\r\n    width: 40px !important; height: 40px !important;\r\n  }\r\n  table[class=3D\"body\"] center {\r\n    min-width: 0 !important;\r\n  }\r\n  table[class=3D\"body\"] .container {\r\n    width: 95% !important;\r\n  }\r\n  table[class=3D\"body\"] .row {\r\n    width: 100% !important; display: block !important;\r\n  }\r\n  table[class=3D\"body\"] .wrapper {\r\n    display: block !important; padding-right: 0 !important;\r\n  }\r\n  table[class=3D\"body\"] .columns {\r\n    table-layout: fixed !important; float: none !important; width: 100% !im=\r\nportant; padding-right: 0px !important; padding-left: 0px !important; displ=\r\nay: block !important;\r\n  }\r\n  table[class=3D\"body\"] .column {\r\n    table-layout: fixed !important; float: none !important; width: 100% !im=\r\nportant; padding-right: 0px !important; padding-left: 0px !important; displ=\r\nay: block !important;\r\n  }\r\n  table[class=3D\"body\"] .wrapper.first .columns {\r\n    display: table !important;\r\n  }\r\n  table[class=3D\"body\"] .wrapper.first .column {\r\n    display: table !important;\r\n  }\r\n  table[class=3D\"body\"] table.columns td {\r\n    width: 100% !important;\r\n  }\r\n  table[class=3D\"body\"] table.column td {\r\n    width: 100% !important;\r\n  }\r\n  table[class=3D\"body\"] .columns td.twelve {\r\n    width: 100% !important;\r\n  }\r\n  table[class=3D\"body\"] .column td.twelve {\r\n    width: 100% !important;\r\n  }\r\n  table[class=3D\"body\"] table.columns td.expander {\r\n    width: 1px !important;\r\n  }\r\n  div[class=3D\"panel\"] {\r\n    padding: 12px !important;\r\n  }\r\n  td[class=3D\"panel\"] {\r\n    padding: 12px !important;\r\n  }\r\n  table[class=3D\"panel\"] {\r\n    padding: 12px !important;\r\n  }\r\n  table[class=3D\"body\"] img.logo-wordmark {\r\n    width: 102px !important;\r\n  }\r\n  table[class=3D\"body\"] img.logo-invertocat {\r\n    width: 40px !important;\r\n  }\r\n}\r\n</style>\r\n<table class=3D\"body\" style=3D\"background: #fafafa; border-collapse: collap=\r\nse; border-spacing: 0; color: #333333; font-family: 'Helvetica Neue', Helve=\r\ntica, Arial, sans-serif; font-size: 14px; font-weight: normal; height: 100%=\r\n; line-height: 20px; margin: 0; padding: 0; text-align: center; vertical-al=\r\nign: top; width: 100%\" bgcolor=3D\"#fafafa\">\r\n<tbody>\r\n<tr style=3D\"padding: 0; text-align: center; vertical-align: top\" align=3D\"=\r\ncenter\">\r\n<td class=3D\"center\" align=3D\"center\" valign=3D\"top\" style=3D\"-moz-hyphens:=\r\n auto; -webkit-hyphens: auto; border-collapse: collapse !important; color: =\r\n#333333; font-family: 'Helvetica Neue', Helvetica, Arial, sans-serif; font-=\r\nsize: 14px; font-weight: normal; hyphens: auto; line-height: 20px; margin: =\r\n0; padding: 0; text-align: center; vertical-align: top; word-break: break-w=\r\nord\">\r\n<center style=3D\"min-width: 580px; width: 100%\"><!--email content-->\r\n<table class=3D\"row header\" style=3D\"border-collapse: collapse; border-spac=\r\ning: 0; padding: 0px; position: relative; text-align: center; vertical-alig=\r\nn: top; width: 100%\">\r\n<tbody>\r\n<tr style=3D\"padding: 0; text-align: center; vertical-align: top\" align=3D\"=\r\ncenter\">\r\n<td class=3D\"center\" align=3D\"center\" style=3D\"-moz-hyphens: auto; -webkit-=\r\nhyphens: auto; border-collapse: collapse !important; color: #333333; font-f=\r\namily: 'Helvetica Neue', Helvetica, Arial, sans-serif; font-size: 14px; fon=\r\nt-weight: normal; hyphens: auto; line-height: 20px; margin: 0; padding: 0; =\r\ntext-align: center; vertical-align: top; word-break: break-word\" valign=3D\"=\r\ntop\">\r\n<center style=3D\"min-width: 580px; width: 100%\">\r\n<table class=3D\"container\" style=3D\"border-collapse: collapse; border-spaci=\r\nng: 0; margin: 0 auto; padding: 0; text-align: inherit; vertical-align: top=\r\n; width: 580px\">\r\n<tbody>\r\n<tr style=3D\"padding: 0; text-align: center; vertical-align: top\" align=3D\"=\r\ncenter\">\r\n<td class=3D\"wrapper last\" style=3D\"-moz-hyphens: auto; -webkit-hyphens: au=\r\nto; border-collapse: collapse !important; color: #333333; font-family: 'Hel=\r\nvetica Neue', Helvetica, Arial, sans-serif; font-size: 14px; font-weight: n=\r\normal; hyphens: auto; line-height: 20px; margin: 0; padding: 0 0px 0 0; pos=\r\nition: relative; text-align: center; vertical-align: top; word-break: break=\r\n-word\" align=3D\"center\" valign=3D\"top\">\r\n<table class=3D\"twelve columns\" style=3D\"border-collapse: collapse; border-=\r\nspacing: 0; margin: 0 auto; padding: 0; text-align: center; vertical-align:=\r\n top; width: 540px\">\r\n<tbody>\r\n<tr style=3D\"padding: 0; text-align: center; vertical-align: top\" align=3D\"=\r\ncenter\">\r\n<td style=3D\"-moz-hyphens: auto; -webkit-hyphens: auto; border-collapse: co=\r\nllapse !important; color: #333333; font-family: 'Helvetica Neue', Helvetica=\r\n, Arial, sans-serif; font-size: 14px; font-weight: normal; hyphens: auto; l=\r\nine-height: 20px; margin: 0; padding: 0px 0px 10px; text-align: center; ver=\r\ntical-align: top; word-break: break-word\" align=3D\"center\" valign=3D\"top\">\r\n<div class=3D\"mark\" style=3D\"text-align: center\" align=3D\"center\"><!-- add =\r\nUTM params to URL --><a href=3D\"https://github.com\" style=3D\"color: #4183C4=\r\n; text-decoration: none\"><img alt=3D\"GitHub, Inc.\" class=3D\"center logo-wor=\r\ndmark\" src=3D\"https://github.githubassets.com/images/email/global/wordmark.=\r\npng\" width=3D\"102\" height=3D\"28\" style=3D\"-ms-interpolation-mode: bicubic; =\r\nborder: none; float: none; margin: 0 auto; max-width: 100%; outline: none; =\r\npadding: 25px 0 17px; text-align: center; text-decoration: none; width: aut=\r\no\" align=3D\"none\">\r\n</a></div>\r\n</td>\r\n<td class=3D\"expander\" style=3D\"-moz-hyphens: auto; -webkit-hyphens: auto; =\r\nborder-collapse: collapse !important; color: #333333; font-family: 'Helveti=\r\nca Neue', Helvetica, Arial, sans-serif; font-size: 14px; font-weight: norma=\r\nl; hyphens: auto; line-height: 20px; margin: 0; padding: 0; text-align: cen=\r\nter; vertical-align: top; visibility: hidden; width: 0px; word-break: break=\r\n-word\" align=3D\"center\" valign=3D\"top\">\r\n</td>\r\n</tr>\r\n</tbody>\r\n</table>\r\n<!--/.twelve.columns--></td>\r\n</tr>\r\n</tbody>\r\n</table>\r\n<!--/.container--></center>\r\n</td>\r\n</tr>\r\n</tbody>\r\n</table>\r\n<!--/.row.header-->\r\n<table class=3D\"container\" style=3D\"border-collapse: collapse; border-spaci=\r\nng: 0; margin: 0 auto; padding: 0; text-align: inherit; vertical-align: top=\r\n; width: 580px\">\r\n<tbody>\r\n<tr style=3D\"padding: 0; text-align: center; vertical-align: top\" align=3D\"=\r\ncenter\">\r\n<td style=3D\"-moz-hyphens: auto; -webkit-hyphens: auto; border-collapse: co=\r\nllapse !important; color: #333333; font-family: 'Helvetica Neue', Helvetica=\r\n, Arial, sans-serif; font-size: 14px; font-weight: normal; hyphens: auto; l=\r\nine-height: 20px; margin: 0; padding: 0; text-align: center; vertical-align=\r\n: top; word-break: break-word\" align=3D\"center\" valign=3D\"top\">\r\n<table class=3D\"row\" style=3D\"border-collapse: collapse; border-spacing: 0;=\r\n display: block; padding: 0px; position: relative; text-align: center; vert=\r\nical-align: top; width: 100%\">\r\n<tbody>\r\n<tr style=3D\"padding: 0; text-align: center; vertical-align: top\" align=3D\"=\r\ncenter\">\r\n<td class=3D\"wrapper last\" style=3D\"-moz-hyphens: auto; -webkit-hyphens: au=\r\nto; border-collapse: collapse !important; color: #333333; font-family: 'Hel=\r\nvetica Neue', Helvetica, Arial, sans-serif; font-size: 14px; font-weight: n=\r\normal; hyphens: auto; line-height: 20px; margin: 0; padding: 0 0px 0 0; pos=\r\nition: relative; text-align: center; vertical-align: top; word-break: break=\r\n-word\" align=3D\"center\" valign=3D\"top\">\r\n<div class=3D\"panel\" style=3D\"background: #ffffff; border-radius: 3px; bord=\r\ner: 1px solid #dddddd; box-shadow: 0 1px 3px rgba(0,0,0,0.05); padding: 20p=\r\nx\">\r\n<table class=3D\"twelve columns\" style=3D\"border-collapse: collapse; border-=\r\nspacing: 0; margin: 0 auto; padding: 0; text-align: center; vertical-align:=\r\n top; width: 540px\">\r\n<tbody>\r\n<tr style=3D\"padding: 0; text-align: center; vertical-align: top\" align=3D\"=\r\ncenter\">\r\n<td style=3D\"-moz-hyphens: auto; -webkit-hyphens: auto; border-collapse: co=\r\nllapse !important; color: #333333; font-family: 'Helvetica Neue', Helvetica=\r\n, Arial, sans-serif; font-size: 14px; font-weight: normal; hyphens: auto; l=\r\nine-height: 20px; margin: 0; padding: 0px 0px 0; text-align: center; vertic=\r\nal-align: top; word-break: break-word\" align=3D\"center\" valign=3D\"top\">\r\n<div class=3D\"email-content\">\r\n<div class=3D\"org-content-header\" style=3D\"padding: 15px 0 10px\"><img class=\r\n=3D\"avatar\" src=3D\"https://avatars1.githubusercontent.com/u/61407870?s=3D12=\r\n0&amp;v=3D4\" width=3D\"60\" height=3D\"60\" alt=3D\"@20192021855-DCAN\" style=3D\"=\r\n-ms-interpolation-mode: bicubic; -webkit-border-radius: 3px; border-radius:=\r\n 3px; max-width: 100%; outline: none; overflow: hidden; text-decoration: no=\r\nne; width: auto\">\r\n<img class=3D\"content-header-octicon\" alt=3D\"plus\" src=3D\"https://github.gi=\r\nthubassets.com/images/email/organization/octicon-plus.png\" height=3D\"60\" st=\r\nyle=3D\"-ms-interpolation-mode: bicubic; max-width: 100%; outline: none; tex=\r\nt-decoration: none; width: auto\">\r\n<img class=3D\"avatar\" src=3D\"https://avatars3.githubusercontent.com/u/32518=\r\n669?s=3D120&amp;v=3D4\" width=3D\"60\" height=3D\"60\" alt=3D\"@12Jack21\" style=\r\n=3D\"-ms-interpolation-mode: bicubic; -webkit-border-radius: 3px; border-rad=\r\nius: 3px; max-width: 100%; outline: none; overflow: hidden; text-decoration=\r\n: none; width: auto\">\r\n</div>\r\n<h1 class=3D\"primary-heading\" style=3D\"color: #333; font-family: 'Helvetica=\r\n Neue',Helvetica,Arial,sans-serif; font-size: 24px; font-weight: 300; line-=\r\nheight: 1.2; margin: 10px 0 25px; padding: 0; text-align: center; word-brea=\r\nk: normal\" align=3D\"center\">\r\n@woods321 has invited you to join the<br>\r\n<strong>@20192021855-DCAN</strong> organization</h1>\r\n<hr class=3D\"rule\" style=3D\"background: #d9d9d9; border: none; color: #d9d9=\r\nd9; height: 1px; margin: 20px 0\">\r\n<p style=3D\"color: #333; font-family: 'Helvetica Neue', Helvetica, Arial, s=\r\nans-serif; font-size: 14px; font-weight: normal; hyphens: none; line-height=\r\n: 20px; margin: 15px 0 5px; padding: 0; text-align: left; word-wrap: normal=\r\n\" align=3D\"left\">\r\nHi <strong>Johnson</strong>! </p>\r\n<p style=3D\"color: #333; font-family: 'Helvetica Neue', Helvetica, Arial, s=\r\nans-serif; font-size: 14px; font-weight: normal; hyphens: none; line-height=\r\n: 20px; margin: 15px 0 5px; padding: 0; text-align: left; word-wrap: normal=\r\n\" align=3D\"left\">\r\n@<a href=3D\"https://github.com/woods321\" style=3D\"color: #4183C4; text-deco=\r\nration: none\">woods321</a> has invited you to join the @20192021855-DCAN or=\r\nganization on GitHub. Head over to\r\n<a href=3D\"https://github.com/20192021855-DCAN\" style=3D\"color: #4183C4; te=\r\nxt-decoration: none\">\r\nhttps://github.com/20192021855-DCAN</a> to check out @20192021855-DCAN=E2=\r\n=80=99s profile.\r\n</p>\r\n<!-- note: VML markup is fallback for Outlook 2007, 2010, and 2013; see htt=\r\np://buttons.cm/ -->\r\n<div class=3D\"cta-button-wrap cta-button-wrap-centered\" style=3D\"color: #ff=\r\nffff; padding: 20px 0 25px; text-align: center\" align=3D\"center\">\r\n<!--[if mso]>\r\n<p style=3D\"line-height:0px;height:0;font-size:1px;margin:0;padding:0;\">&nb=\r\nsp;</p>\r\n<v:roundrect xmlns:v=3D\"urn:schemas-microsoft-com:vml\" xmlns:w=3D\"urn:schem=\r\nas-microsoft-com:office:word\" href=3D\"https://github.com/orgs/20192021855-D=\r\nCAN/invitation?via_email=3D1\" style=3D\"height:40px;v-text-anchor:middle;wid=\r\nth:200px;\" arcsize=3D\"8%\" stroke=3D\"f\" fillcolor=3D\"#4183C4\">\r\n  <w:anchorlock/>\r\n  <center>\r\n<![endif]--><a class=3D\"cta-button\" href=3D\"https://github.com/orgs/2019202=\r\n1855-DCAN/invitation?via_email=3D1\" style=3D\"-webkit-border-radius: 5px; -w=\r\nebkit-text-size-adjust: none; background: #28A73F linear-gradient(-180deg, =\r\n#34d058 0%, #28a745 90%); border-radius: 5px; box-shadow: 0px 3px 0px #2558=\r\n8c; color: #fff; display: inline-block; font-family: 'Helvetica Neue', Helv=\r\netica, Arial, sans-serif; font-size: 14px; font-weight: 600; letter-spacing=\r\n: normal; margin: 0 auto; padding: 6px 12px; text-align: center; text-decor=\r\nation: none; width: auto !important\">Join\r\n @20192021855-DCAN</a><!--[if mso]>\r\n  </center>\r\n</v:roundrect>\r\n<![endif]--> </div>\r\n<p class=3D\"email-body-subtext\" style=3D\"color: #333; font-family: 'Helveti=\r\nca Neue', Helvetica, Arial, sans-serif; font-size: 13px; font-weight: norma=\r\nl; hyphens: none; line-height: 20px; margin: 15px 0 5px; padding: 0; text-a=\r\nlign: left; word-wrap: normal\" align=3D\"left\">\r\n<strong>Note:</strong> If you get a 404 page, make sure you=E2=80=99re sign=\r\ned in as <strong>\r\n12Jack21</strong>. You can also accept the invitation by visiting the organ=\r\nization page directly at\r\n<a href=3D\"https://github.com/20192021855-DCAN\" style=3D\"color: #4183C4; te=\r\nxt-decoration: none\">\r\nhttps://github.com/20192021855-DCAN</a>. If @woods321 is sending you too ma=\r\nny emails, you can\r\n<a href=3D\"https://github.com/settings/blocked_users?block_user=3Dwoods321\"=\r\n style=3D\"color: #4183C4; text-decoration: none\">\r\nblock them</a> or <a href=3D\"https://github.com/contact/report-abuse?report=\r\n=3Dwoods321\" style=3D\"color: #4183C4; text-decoration: none\">\r\nreport them for abuse</a>. </p>\r\n<hr class=3D\"rule\" style=3D\"background: #d9d9d9; border: none; color: #d9d9=\r\nd9; height: 1px; margin: 20px 0\">\r\n<p class=3D\"email-text-small email-text-gray\" style=3D\"color: #777777; font=\r\n-family: 'Helvetica Neue', Helvetica, Arial, sans-serif; font-size: 12px; f=\r\nont-weight: normal; hyphens: none; line-height: 20px; margin: 15px 0 5px; p=\r\nadding: 0; text-align: left; word-wrap: normal\" align=3D\"left\">\r\nButton not working? Paste the following link into your browser:<br>\r\n<a href=3D\"https://github.com/orgs/20192021855-DCAN/invitation?via_email=3D=\r\n1\" style=3D\"color: #4183C4; text-decoration: none\">https://github.com/orgs/=\r\n20192021855-DCAN/invitation?via_email=3D1</a>\r\n</p>\r\n<p class=3D\"email-text-small email-text-gray\" style=3D\"color: #777777; font=\r\n-family: 'Helvetica Neue', Helvetica, Arial, sans-serif; font-size: 12px; f=\r\nont-weight: normal; hyphens: none; line-height: 20px; margin: 15px 0 5px; p=\r\nadding: 0; text-align: left; word-wrap: normal\" align=3D\"left\">\r\nYou=E2=80=99re receiving this email because @woods321 invited you to an org=\r\nanization on GitHub.\r\n</p>\r\n</div>\r\n<!--/.content--></td>\r\n<td class=3D\"expander\" style=3D\"-moz-hyphens: auto; -webkit-hyphens: auto; =\r\nborder-collapse: collapse !important; color: #333333; font-family: 'Helveti=\r\nca Neue', Helvetica, Arial, sans-serif; font-size: 14px; font-weight: norma=\r\nl; hyphens: auto; line-height: 20px; margin: 0; padding: 0; text-align: cen=\r\nter; vertical-align: top; visibility: hidden; width: 0px; word-break: break=\r\n-word\" align=3D\"center\" valign=3D\"top\">\r\n</td>\r\n</tr>\r\n</tbody>\r\n</table>\r\n<!--/.twelve-columns--></div>\r\n<!--/.panel--></td>\r\n</tr>\r\n</tbody>\r\n</table>\r\n<!--/.row--></td>\r\n</tr>\r\n</tbody>\r\n</table>\r\n<!--/.container-->\r\n<table class=3D\"row layout-footer\" style=3D\"border-collapse: collapse; bord=\r\ner-spacing: 0; padding: 0px; position: relative; text-align: center; vertic=\r\nal-align: top; width: 100%\">\r\n<tbody>\r\n<tr style=3D\"padding: 0; text-align: center; vertical-align: top\" align=3D\"=\r\ncenter\">\r\n<td class=3D\"center\" align=3D\"center\" style=3D\"-moz-hyphens: auto; -webkit-=\r\nhyphens: auto; border-collapse: collapse !important; color: #333333; font-f=\r\namily: 'Helvetica Neue', Helvetica, Arial, sans-serif; font-size: 14px; fon=\r\nt-weight: normal; hyphens: auto; line-height: 20px; margin: 0; padding: 0; =\r\ntext-align: center; vertical-align: top; word-break: break-word\" valign=3D\"=\r\ntop\">\r\n<center style=3D\"min-width: 580px; width: 100%\">\r\n<table class=3D\"container\" style=3D\"border-collapse: collapse; border-spaci=\r\nng: 0; margin: 0 auto; padding: 0; text-align: inherit; vertical-align: top=\r\n; width: 580px\">\r\n<tbody>\r\n<tr style=3D\"padding: 0; text-align: center; vertical-align: top\" align=3D\"=\r\ncenter\">\r\n<td class=3D\"wrapper last\" style=3D\"-moz-hyphens: auto; -webkit-hyphens: au=\r\nto; border-collapse: collapse !important; color: #333333; font-family: 'Hel=\r\nvetica Neue', Helvetica, Arial, sans-serif; font-size: 14px; font-weight: n=\r\normal; hyphens: auto; line-height: 20px; margin: 0; padding: 0 0px 0 0; pos=\r\nition: relative; text-align: center; vertical-align: top; word-break: break=\r\n-word\" align=3D\"center\" valign=3D\"top\">\r\n<table class=3D\"twelve columns\" style=3D\"border-collapse: collapse; border-=\r\nspacing: 0; margin: 0 auto; padding: 0; text-align: center; vertical-align:=\r\n top; width: 540px\">\r\n<tbody>\r\n<tr style=3D\"padding: 0; text-align: center; vertical-align: top\" align=3D\"=\r\ncenter\">\r\n<td style=3D\"-moz-hyphens: auto; -webkit-hyphens: auto; border-collapse: co=\r\nllapse !important; color: #333333; font-family: 'Helvetica Neue', Helvetica=\r\n, Arial, sans-serif; font-size: 14px; font-weight: normal; hyphens: auto; l=\r\nine-height: 20px; margin: 0; padding: 0px 0px 10px; text-align: center; ver=\r\ntical-align: top; word-break: break-word\" align=3D\"center\" valign=3D\"top\">\r\n<div class=3D\"footer-links\" style=3D\"padding: 20px 0; text-align: center\" a=\r\nlign=3D\"center\">\r\n<!-- links need UTM params -->\r\n<p class=3D\"footer-text\" style=3D\"color: #999; font-family: 'Helvetica Neue=\r\n', Helvetica, Arial, sans-serif; font-size: 12px; font-weight: normal; hyph=\r\nens: none; line-height: 20px; margin: 0; padding: 0; text-align: center; wo=\r\nrd-wrap: normal\" align=3D\"center\">\r\n<a href=3D\"https://github.com/orgs/20192021855-DCAN/opt-out\" style=3D\"color=\r\n: #4183C4; text-decoration: none\">Opt out</a> of future invitations from th=\r\nis organization.</p>\r\n<p class=3D\"footer-text\" style=3D\"color: #999; font-family: 'Helvetica Neue=\r\n', Helvetica, Arial, sans-serif; font-size: 12px; font-weight: normal; hyph=\r\nens: none; line-height: 20px; margin: 0; padding: 0; text-align: center; wo=\r\nrd-wrap: normal\" align=3D\"center\">\r\n<a href=3D\"https://github.com/settings/emails\" style=3D\"color: #4183C4; tex=\r\nt-decoration: none\">Manage your GitHub email preferences</a></p>\r\n<p class=3D\"footer-text\" style=3D\"color: #999; font-family: 'Helvetica Neue=\r\n', Helvetica, Arial, sans-serif; font-size: 12px; font-weight: normal; hyph=\r\nens: none; line-height: 20px; margin: 0; padding: 0; text-align: center; wo=\r\nrd-wrap: normal\" align=3D\"center\">\r\n<a href=3D\"https://help.github.com/articles/github-terms-of-service/\" style=\r\n=3D\"color: #4183C4; text-decoration: none\">Terms</a> =E2=80=A2\r\n<a href=3D\"https://help.github.com/articles/github-privacy-policy/\" style=\r\n=3D\"color: #4183C4; text-decoration: none\">\r\nPrivacy</a> =E2=80=A2 <a href=3D\"https://github.com/login\" style=3D\"color: =\r\n#4183C4; text-decoration: none\">\r\nLog in to GitHub</a> </p>\r\n</div>\r\n<div class=3D\"content\" style=3D\"margin: 0 0 15px\"><!-- add UTM params to UR=\r\nL --><a href=3D\"https://github.com\" style=3D\"color: #4183C4; text-decoratio=\r\nn: none\"><img class=3D\"logo-invertocat\" src=3D\"https://github.githubassets.=\r\ncom/images/email/global/footer-mark.png\" width=3D\"40\" height=3D\"38\" style=\r\n=3D\"-ms-interpolation-mode: bicubic; border: none; max-width: 100%; outline=\r\n: none; text-decoration: none; width: auto\">\r\n</a></div>\r\n<div class=3D\"content\" style=3D\"margin: 0 0 15px\">\r\n<p class=3D\"footer-text\" style=3D\"color: #999; font-family: 'Helvetica Neue=\r\n', Helvetica, Arial, sans-serif; font-size: 12px; font-weight: normal; hyph=\r\nens: none; line-height: 20px; margin: 0; padding: 0; text-align: center; wo=\r\nrd-wrap: normal\" align=3D\"center\">\r\nGitHub, Inc.</p>\r\n<p class=3D\"footer-text\" style=3D\"color: #999; font-family: 'Helvetica Neue=\r\n', Helvetica, Arial, sans-serif; font-size: 12px; font-weight: normal; hyph=\r\nens: none; line-height: 20px; margin: 0; padding: 0; text-align: center; wo=\r\nrd-wrap: normal\" align=3D\"center\">\r\n88 Colin P Kelly Jr Street</p>\r\n<p class=3D\"footer-text\" style=3D\"color: #999; font-family: 'Helvetica Neue=\r\n', Helvetica, Arial, sans-serif; font-size: 12px; font-weight: normal; hyph=\r\nens: none; line-height: 20px; margin: 0; padding: 0; text-align: center; wo=\r\nrd-wrap: normal\" align=3D\"center\">\r\nSan Francisco, CA 94107</p>\r\n</div>\r\n</td>\r\n<td class=3D\"expander\" style=3D\"-moz-hyphens: auto; -webkit-hyphens: auto; =\r\nborder-collapse: collapse !important; color: #333333; font-family: 'Helveti=\r\nca Neue', Helvetica, Arial, sans-serif; font-size: 14px; font-weight: norma=\r\nl; hyphens: auto; line-height: 20px; margin: 0; padding: 0; text-align: cen=\r\nter; vertical-align: top; visibility: hidden; width: 0px; word-break: break=\r\n-word\" align=3D\"center\" valign=3D\"top\">\r\n</td>\r\n</tr>\r\n</tbody>\r\n</table>\r\n<!--/.twelve.columns--></td>\r\n</tr>\r\n</tbody>\r\n</table>\r\n<!--/.container--></center>\r\n</td>\r\n</tr>\r\n</tbody>\r\n</table>\r\n<!--/.row.footer--><!--/email content--></center>\r\n</td>\r\n</tr>\r\n</tbody>\r\n</table>\r\n<!--/.body-->\r\n</body>\r\n</html>\r\n";

        #region SMTP/POP3 身份验证
        static void Validate_Example()
        {
            MailUtil.LoginInfo info_smtp 
                = new MailUtil.LoginInfo() { account = "alertdoll@163.com",
                passwd = "ybgissocute2020",
                site = "smtp.163.com:25" };
            if (MailUtil.validate_account_smtp(info_smtp))
            {
                Console.WriteLine("Succ");
            }
            else
            {
                Console.WriteLine("Fail");
            }

            MailUtil.LoginInfo info_pop3 = new MailUtil.LoginInfo()
            {
                account = "alertdoll@163.com",
                passwd = "ybgissocute2020",
                site = "pop.163.com:110"
            };
            if (MailUtil.validate_account_pop3(info_pop3))
            {
                Console.WriteLine("Succ");
            }
            else
            {
                Console.WriteLine("Fail");
            }

            Console.ReadKey();
        }
        #endregion

        #region SMTP send mail
        static void SendMail_Example()
        {
            MailUtil.LoginInfo info_smtp = new MailUtil.LoginInfo()
            {
                account = "alertdoll@163.com",
                passwd = "ybgissocute2020",
                site = "smtp.163.com:25"
            };

            MailUtil.MailInfo mail_info = new MailUtil.MailInfo()
            {
                from="alertdoll@163.com",
                to="ale_li_pona@163.com",
                cc="alertdoll@163.com",
                subject="test",
                body="Haha",
            };

            Int32 result = MailUtil.login_send_mail(info_smtp, mail_info);

            Console.WriteLine(result);

            Console.ReadKey();
        }

        static void SendMail_Example_Extern()
        {
            MailUtil.LoginInfo info_smtp = new MailUtil.LoginInfo()
            {
                account = "alertdoll@163.com",
                passwd = "ybgissocute2020",
                site = "smtp.163.com:25"
            };

            MailUtil.MailInfo mail_info = new MailUtil.MailInfo()
            {
                from = "alertdoll@163.com",
                to = "ale_li_pona@163.com",
                cc = "alertdoll@163.com",
                subject = "test",
                body = mailstr,
            };

            Int32 result = MailUtil.login_send_mail_extern(info_smtp, mail_info);

            Console.WriteLine(result);

            Console.ReadKey();
        }
        #endregion

        #region POP3 get the number of mails
        static void GetNumMails_Example()
        {
            MailUtil.LoginInfo info_pop3 = new MailUtil.LoginInfo()
            {
                account = "alertdoll@163.com",
                passwd = "ybgissocute2020",
                site = "pop.163.com:110"
            };

            Int32 num = MailUtil.get_num_mails(info_pop3);
            Console.WriteLine(num);

            Console.ReadKey();
        }
        #endregion

        #region POP3 receive mail
        static void ReceiveMail_Example()
        {
            MailUtil.LoginInfo info_pop3 = new MailUtil.LoginInfo()
            {
                account = "alertdoll@163.com",
                passwd = "ybgissocute2020",
                site = "pop.163.com:110"
            };

            MailStr mail_str = new MailStr(info_pop3, 3);
            Console.WriteLine(mail_str);

            //MailUtil.ResultStruct rs = MailUtil.get_simple_email(info_pop3, 3);
            //Console.WriteLine(Marshal.PtrToStringAnsi(rs.mail_string));

            //string temp = MailUtil.rustffi_get_version(info_pop3, 3);
            //string ver = (string)temp.Clone();
            //MailUtil.rustffi_get_version_free(temp);
            //Console.WriteLine(ver);

            Console.ReadKey();
        }
        #endregion

        public static void Main(string[] args)
        {
            //Validate_Example();

            //SendMail_Example();

            //GetNumMails_Example();

            //ReceiveMail_Example();

            SendMail_Example_Extern();
        }

    }
}
