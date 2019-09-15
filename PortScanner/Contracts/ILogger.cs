using PortScanner.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortScanner.Contracts
{
    public interface ILogger
    {
        void Log(string logMessage, LogSeverity severity);
        Task LogAsync(string logMessage, LogSeverity severity);
    }
}
