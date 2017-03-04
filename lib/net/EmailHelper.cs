﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.IO;
using System.Xml;
using System.Reflection;
using Lib.core;
using OpenPop.Pop3;
using Lib.helper;

namespace Lib.net
{
    public class EmailModel
    {
        public EmailModel()
        {
            this.EnableSSL = false;
            this.SendPort = 25;
            this.TimeOut = 1000 * 10;
        }

        //发送设置
        public string SmtpServer { get; set; }

        public string PopServer { get; set; }

        public string UserName { get; set; }

        public string SenderName { get; set; }

        public string Address { get; set; }

        public string Password { get; set; }

        public bool EnableSSL { get; set; }

        public int SendPort { get; set; }

        public int TimeOut { get; set; }

        public List<string> ToList { get; set; }

        public List<string> CcList { get; set; }

        public string Subject { get; set; }

        public string MailBody { get; set; }

        public string[] File_attachments { get; set; }
    }

    /// <summary>
    ///Send_Emails 的摘要说明
    /// </summary>
    public static class EmailSender
    {
        private static System.Net.Mail.MailMessage BuildMail(EmailModel model)
        {
            System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
            //收件人
            if (ValidateHelper.IsPlumpList(model.ToList))
            {
                model.ToList.ForEach(delegate (string to)
                {
                    if (ValidateHelper.IsEmail(to))
                    {
                        mail.To.Add(to);
                    }
                });
            }
            //抄送人
            if (ValidateHelper.IsPlumpList(model.CcList))
            {
                model.CcList.ForEach(delegate (string cc)
                {
                    if (ValidateHelper.IsEmail(cc))
                    {
                        mail.CC.Add(cc);
                    }
                });
            }
            mail.From = new MailAddress(model.Address, model.SenderName, Encoding.UTF8);
            mail.Subject = model.Subject;
            mail.SubjectEncoding = Encoding.UTF8;
            mail.Body = model.MailBody;
            mail.BodyEncoding = Encoding.UTF8;
            mail.IsBodyHtml = true;//发送html
            mail.Priority = System.Net.Mail.MailPriority.Normal;
            return mail;
        }

        private static SmtpClient BuildSmtp(EmailModel model)
        {
            SmtpClient smtp = new SmtpClient();
            smtp.Credentials = new NetworkCredential(model.UserName, model.Password);
            smtp.Host = model.SmtpServer;
            smtp.EnableSsl = model.EnableSSL;
            smtp.Port = model.SendPort;
            smtp.Timeout = model.TimeOut;
            return smtp;
        }

        public static bool SendMail(EmailModel model)
        {
            System.Net.Mail.MailMessage mail = null;
            SmtpClient smtp = null;
            try
            {
                mail = BuildMail(model);
                smtp = BuildSmtp(model);
                smtp.Send(mail);
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (smtp != null) { smtp.Dispose(); }
                if (mail != null) { mail.Dispose(); }
            }
        }

        /// <summary>
        /// 接受邮件，处理所有正确存在邮件
        /// </summary>
        public static List<OpenPop.Mime.Message> ReceiveMail(EmailModel model, bool delete)
        {
            var popClient = new Pop3Client();
            var list = new List<OpenPop.Mime.Message>();
            try
            {
                popClient.Connect(model.PopServer, 110, false);
                popClient.Authenticate(model.UserName, model.Password);
                int count = popClient.GetMessageCount();

                for (int i = count; i >= 1; --i)
                {
                    var msg = popClient.GetMessage(i);
                    list.Add(msg);
                    if (delete)
                    {
                        popClient.DeleteMessage(i); //邮件保存成功，删除服务器备份
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                popClient.Disconnect();
            }
            return list;
        }

    }
}