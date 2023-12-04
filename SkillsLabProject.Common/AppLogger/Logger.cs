using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillsLabProject.Common.AppLogger
{
    public class Logger : ILogger
    {
        private readonly string logFilePath;

        public Logger(string filePath = "Logs\\log.txt")
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            logFilePath = Path.Combine(baseDirectory, filePath);
        }

        public void Log(string message)
        {
            string logMessage = $"{message}";

            try
            {
                if (!File.Exists(logFilePath))
                {
                    File.Create(logFilePath);
                }

                using (StreamWriter writer = File.AppendText(logFilePath))
                {
                    writer.WriteLine(logMessage);
                }
            }
            catch (Exception error)
            {
                Console.WriteLine($"Error writing to log file: {error.Message}");
                throw;
            }
        }

    }
}
