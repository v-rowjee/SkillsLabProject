using SkillsLabProject.Common.Email;
using System.Threading.Tasks;

namespace SkillsLabProject.BL.Services
{
    public interface IEmailService
    {
        void SendEmail(string subject, string body, string recipientEmail, string ccEmail);
    }

    public class EmailService : IEmailService
    {
        public void SendEmail(string subject, string body, string recipientEmail, string ccEmail)
        {
            SmtpEmailClient smtp = new SmtpEmailClient();
            Task.Run(() => smtp.SendEmailAsync(recipientEmail, ccEmail, subject, body));
        }
    }
}
