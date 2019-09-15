using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PortScanner
{
    public static class Validation
    {
        public static bool ValidateIPs(string beginIPStr, string endIPStr)
        {
            if (string.IsNullOrEmpty(beginIPStr) && string.IsNullOrEmpty(endIPStr))
            {
                throw new ArgumentException("Please enter begin and end IPs");
            }
            else if (string.IsNullOrEmpty(beginIPStr))
            {
                throw new ArgumentException("Please enter begin IP");
            }
            else if (string.IsNullOrEmpty(endIPStr))
            {
                endIPStr = beginIPStr;
            }
            IPAddress beginIP;
            IPAddress endIP;
            var isBeginIPValid = PortScanning.TryParse(beginIPStr, out beginIP);
            var isEndIPValid = PortScanning.TryParse(endIPStr, out endIP);

            if (!isBeginIPValid)
            {
                throw new ArgumentException(beginIPStr + " is not a valid IP");
            }

            if (!isEndIPValid)
            {
                throw new ArgumentException(endIPStr + " is not a valid IP");
            }

            return true;
        }
    }
}
