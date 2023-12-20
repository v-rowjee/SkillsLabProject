using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using SkillsLabProject.Common.Models.ViewModels;

namespace SkillsLabProject.BL.Services
{
    public interface IEmailService
    {
        bool SendEmail(EmailViewModel model);
    }

    public class EmailService : IEmailService
    {
        private readonly string _subject = "Training Enrollment Approved";

        public bool SendEmail(EmailViewModel model)
        {
            string body = $@"<html><body>
                              <p>Dear {model.Employee.FirstName},</p>
                              <p>Congratulations! Your {model.Training.Title} training enrollment has been approved by your manager {model.Manager.FirstName} {model.Manager.LastName}, today, {DateTime.Now.ToString("ddd, dd MMMM yyyy")} at {DateTime.Now.ToString("hh:mm tt")}.</p>
                              <p>Please check the details and make necessary arrangements.</p>

                              <p><strong>Best regards,</strong><br/>{model.Employee.Department.Title}, SkillsLab</p>
                              </body></html>";
            string senderEmail = ConfigurationManager.AppSettings["FirebaseEmail"].ToString();
            try
            {
                using (SmtpClient client = new SmtpClient("smtp.gmail.com"))
                {
                    client.Port = 587;
                    client.EnableSsl = true;
                    client.Credentials = new NetworkCredential(senderEmail, "xuza rdim fkyz iupb");

                    MailMessage message = new MailMessage
                    {
                        From = new MailAddress(senderEmail, model.Employee.Department.Title),
                        Subject = _subject,
                        Body = body,
                        IsBodyHtml = true,
                    };
                    message.To.Add(model.Employee.Email);
                    message.CC.Add(model.Manager.Email);

                    client.Send(message);
                    return true;
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
