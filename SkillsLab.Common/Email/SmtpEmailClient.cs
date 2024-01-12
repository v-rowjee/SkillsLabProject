using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Configuration;
using SkillsLabProject.Common.Exceptions;

namespace SkillsLabProject.Common.Email
{
    public class SmtpEmailClient
    {
        private readonly string _server;
        private readonly int _port;
        private readonly string _senderEmail = ConfigurationManager.AppSettings["AdminEmail"].ToString();
        public SmtpEmailClient() 
        {
            _server = "relay.ceridian.com";
            _port = 587;
        }
        public async Task<bool> SendEmailAsync(string to, string cc, string subject, string body)
        {
            try
            {
                using (SmtpClient client = new SmtpClient(_server))
                {
                    client.Port = _port;
                    client.EnableSsl = true;
                    client.UseDefaultCredentials = true;

                    using (MailMessage message = new MailMessage(_senderEmail, to))
                    {
                        message.CC.Add(cc);
                        message.Subject = subject;
                        message.Body = body;
                        message.IsBodyHtml = true;

                        if (bool.Parse(ConfigurationManager.AppSettings["smtp:Enabled"]))
                        {
                            await client.SendMailAsync(message).ConfigureAwait(false);
                        }
                        return true;
                    };
                }
            }
            catch (Exception error)
            {
                var exception = new CustomException(error);
                exception.Log();
                return false;
            }
        }
    }
}
