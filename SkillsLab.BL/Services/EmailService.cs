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
        Task<bool> SendEmail(string subject, string body, string recipientEmail, string ccEmail);
    }

    public class EmailService : IEmailService
    {
        public async Task<bool> SendEmail(string subject, string body, string recipientEmail, string ccEmail)
        {
            SmtpEmailClient smtp = new SmtpEmailClient();
            return await smtp.SendEmailAsync(recipientEmail, ccEmail, subject, body);
        }
    }
}
