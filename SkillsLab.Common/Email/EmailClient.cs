﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Configuration;

namespace SkillsLabProject.Common.Email
{
    public class SmtpEmailClient
    {
        private readonly string _server;
        private readonly int _port;
        private readonly string _senderEmail;
        private readonly string _senderPassword;
        public SmtpEmailClient() 
        {
            _server = "smtp-mail.outlook.com";
            _port = 587;
            _senderEmail = ConfigurationManager.AppSettings["AdminEmail"].ToString();
            _senderPassword = ConfigurationManager.AppSettings["AdminPassword"].ToString();
        }
        public async Task<bool> SendEmail(string to, string cc, string subject, string body)
        {
            try
            {
                using (SmtpClient client = new SmtpClient(_server))
                {
                    client.Port = _port;
                    client.EnableSsl = true;
                    client.Credentials = new NetworkCredential(_senderEmail, _senderPassword);

                    using (MailMessage message = new MailMessage(_senderEmail, to))
                    {
                        message.CC.Add(cc);
                        message.Subject = subject;
                        message.Body = body;
                        message.IsBodyHtml = true;

                        await client.SendMailAsync(message);
                        return true;
                    };
                }
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }
    }
}
