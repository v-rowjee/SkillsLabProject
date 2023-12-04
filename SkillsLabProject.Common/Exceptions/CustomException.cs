using SkillsLabProject.Common.AppLogger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillsLabProject.Common.Exceptions
{
    public class CustomException : Exception
    {
        private readonly Logger _logger;
        public CustomException(string message, Exception exception) : base(message, exception)
        {
            _logger = new Logger();
        }

        public void Log()
        {
            string fullMessage = "--------------------------------------------------------";
            fullMessage += Environment.NewLine + $"Timestamp: {DateTime.Now}";
            fullMessage += Environment.NewLine + $"Exception Type: {GetType().FullName}";
            fullMessage += Environment.NewLine + $"Message Type: {Message}";
            fullMessage += Environment.NewLine + $"Inner Exception: {InnerException}";
            fullMessage += Environment.NewLine + $"Stack Trace: {StackTrace}";
            fullMessage = "--------------------------------------------------------";
            _logger.Log(fullMessage);
        }
    }
}
