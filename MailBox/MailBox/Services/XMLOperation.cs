using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using MailBox.Models;

namespace MailBox.Services
{
    class XMLOperation
    {
        /**
         * 创建账户信息XML
         */
        static void generateXml()
        {
            if (File.Exists(@"../../Resources/AccountInfos.xml")) return;
            //创建XmlDocument对象
            XmlDocument document = new XmlDocument();
            //xml文档的声明部分
            XmlDeclaration declaration = document.CreateXmlDeclaration("1.0", "UTF-8", "");//xml文档的声明部分
            document.AppendChild(declaration);//添加至XmlDocument对象中
            //创建账户对象
            //创建根节点AccountInfos
            XmlElement AccountInfos = document.CreateElement("AccountInfos");//CreateElement（节点名称）
            //将根节点添加至XML文档中
            document.AppendChild(AccountInfos);
            //保存输出路径
            document.Save(@"../../Resources/AccountInfos.xml");
            Console.WriteLine("创建账户xml");
            ///>
            ///注意要把根节点添加至XML文档中
            ///多层级只需要在其子节点使用AppendChild再次添加即可
            ///使用XmlElement对象的SetAttribute方法设置其属性
            ///在保存路径的时候最好使用异常处理

        }

        /**
         * 添加账号信息节点
         */
         public static void AddAccountNode(AccountInfo accountInfo)
        {
            generateXml();
            XmlDocument xmlDoc = new XmlDocument();
            //加载xml文件
            xmlDoc.Load(@"../../Resources/AccountInfos.xml"); //从指定的位置加载xml文档
            //获取根节点
            XmlNode xmlRoot = xmlDoc.SelectSingleNode("AccountInfos"); //DocumentElement获取文档的根
            XmlElement AccountInfo = xmlDoc.CreateElement("AccountInfo");//CreateElement（节点名称）
            //创建子节点ID
            XmlElement account = xmlDoc.CreateElement("account");
            account.InnerText = accountInfo.Account; //设置其值
            XmlElement password = xmlDoc.CreateElement("password");
            password.InnerText = accountInfo.Password;
            XmlElement popHost = xmlDoc.CreateElement("popHost");
            popHost.InnerText = accountInfo.PopHost;
            XmlElement smtpHost = xmlDoc.CreateElement("smtpHost");
            smtpHost.InnerText = accountInfo.SmtpHost;
            //添加至父节点中
            AccountInfo.AppendChild(account);
            AccountInfo.AppendChild(password);
            AccountInfo.AppendChild(popHost);
            AccountInfo.AppendChild(smtpHost);
            xmlRoot.AppendChild(AccountInfo);
            xmlDoc.Save(@"../../Resources/AccountInfos.xml");
        }

        /**
         * 从XML文件中加载账户信息
         */
        public static ObservableCollection<AccountInfo> loadAccouts()
        {
            ObservableCollection<AccountInfo> accountInfos = new ObservableCollection<AccountInfo>();
            XmlDocument xmlDoc = new XmlDocument();
            //加载xml文件
            xmlDoc.Load(@"../../Resources/AccountInfos.xml"); //从指定的位置加载xml文档
            //获取根节点
            XmlElement xmlRoot = xmlDoc.DocumentElement; //DocumentElement获取文档的跟
            //遍历节点
            foreach (XmlNode node in xmlRoot.ChildNodes)
            {
                //根据节点名称查找节点对象
                accountInfos.Add(new AccountInfo() { 
                    Account= node["account"].InnerText,
                    Password= node["password"].InnerText,
                    PopHost= node["popHost"].InnerText,
                    SmtpHost= node["smtpHost"].InnerText,
                });
                Console.WriteLine(node["account"].InnerText + "\t" + node["password"].InnerText + "\t" + node["popHost"].InnerText + "\t" + node["smtpHost"].InnerText);
            }
            return accountInfos;
        }
    }
}
