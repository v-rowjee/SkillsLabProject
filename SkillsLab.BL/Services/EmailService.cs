using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using SkillsLabProject.Common.Models.ViewModels;

namespace SkillsLabProject.BL.Services
{
    public interface IEmailService
    {
        Task<bool> SendEmail(EmailViewModel model);
    }

    public class EmailService : IEmailService
    {
        private readonly string _subject = "Training Enrollment Approved";

        public async Task<bool> SendEmail(EmailViewModel model)
        {
            string body = $@"<html><body>
                              <p>Dear {model.Employee.FirstName},</p>
                              <p>Congratulations! Your {model.Training.Title} training enrollment has been approved by your manager {model.Manager.FirstName} {model.Manager.LastName}, today, {DateTime.Now.ToString("ddd, dd MMMM yyyy")} at {DateTime.Now.ToString("hh:mm tt")}.</p>
                              <p>Please check the details and make necessary arrangements.</p>

                              <p><strong>Best regards,</strong><br/>{model.Employee.Department.Title}, SkillsLab</p>
                              </body></html>";
            string recipientEmail = model.Employee.Email;
            string senderEmail = ConfigurationManager.AppSettings["AdminEmail"].ToString();
            string senderPassword = ConfigurationManager.AppSettings["AdminPassword"].ToString();
            try
            {
                using (SmtpClient client = new SmtpClient("smtp-mail.outlook.com"))
                {
                    client.Port = 587;
                    client.EnableSsl = true;
                    client.Credentials = new NetworkCredential(senderEmail, senderPassword);

                    using (MailMessage message = new MailMessage(senderEmail, recipientEmail))
                    {
                        message.CC.Add(model.Manager.Email);
                        message.Subject = _subject;
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
