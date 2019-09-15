using PortScanner.Contracts;
using PortScanner.Enums;
using PortScanner.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortScanner.Utility
{
    public class FileLogger : ILogger
    {
        private static string _filePath;
        private readonly string _filePathNonStatic;

        public FileLogger()
        {
            _filePath = ConfigurationSettings.AppSettings["LogFilePath"];
            if (string.IsNullOrEmpty(_filePath))
                _filePath = System.AppDomain.CurrentDomain.BaseDirectory + @"\LogFile.txt";
        }
        public void Log(string logMessage, LogSeverity severity)
        {
            FileIO.WriteToFileThreadSafe(severity.ToString() + ": " + logMessage, _filePath);
        }

        public async Task LogAsync(string logMessage, LogSeverity severity)
        {
            FileIO.WriteToFileThreadSafe(severity.ToString() + ": " + logMessage, _filePath);
        }
    }
}
