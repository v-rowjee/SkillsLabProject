using SkillsLabProject.Common.Log;
using System;
using System.Diagnostics;

namespace SkillsLabProject.Common.Exceptions
{
    public class CustomException : Exception
    {
        private readonly Logger _logger;
        public CustomException(Exception exception) : base(exception.Message, exception)
        {
            _logger = new Logger();
        }

        public void Log()
        {
            string fullMessage = "";
            fullMessage += Environment.NewLine + "--------------------------------------------------------";
            fullMessage += Environment.NewLine + $"Timestamp: {DateTime.Now}";
            fullMessage += Environment.NewLine + $"Exception Type: {GetType().FullName}";
            fullMessage += Environment.NewLine + $"Message Type: {Message}";
            fullMessage += Environment.NewLine + $"Inner Exception: {InnerException}";
            fullMessage += Environment.NewLine + $"Stack Trace: {StackTrace}";
            fullMessage += Environment.NewLine + "--------------------------------------------------------";
            _logger.Log(fullMessage);
        }
    }
}
