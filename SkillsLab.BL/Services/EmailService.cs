using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using SkillsLabProject.Common.Email;
using SkillsLabProject.Common.Models.ViewModels;

namespace SkillsLabProject.BL.Services
{
    public interface IEmailService
    {
        Task<bool> SendEmail(EmailViewModel model);
    }

    public class EmailService : IEmailService
    {
        public async Task<bool> SendEmail(EmailViewModel model)
        {
            string subject = "Training Enrollment Approved";
            string body = $@"<html><body>
                              <p>Dear {model.Employee.FirstName},</p>
                              <p>Congratulations! Your {model.Training.Title} training enrollment has been approved by your manager {model.Manager.FirstName} {model.Manager.LastName}, on {DateTime.Now.ToString("dddd, dd MMMM yyyy")} at {DateTime.Now.ToString("hh:mm tt")}.</p>
                              <p>Please check the details and make necessary arrangements.</p>
                              <p>Best regards,<br/>{model.Employee.Department.Title}, SkillsLab</p>
                              </body></html>";
            string recipientEmail = model.Employee.Email;
            string ccEmail = model.Manager.Email;

            SmtpEmailClient smtp = new SmtpEmailClient();
            return await smtp.SendEmailAsync(recipientEmail, ccEmail, subject, body);
        }
    }
}
