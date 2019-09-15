using System;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PortScanner.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TryParseTest1()
        {
            string ip = "123.3.2.3";
            System.Net.IPAddress returnIP;
            var isValid = PortScanning.TryParse(ip, out returnIP);

            Assert.IsTrue(isValid);
        }

        [TestMethod]
        public void TryParseTest2()
        {
            string ip = "123.3.2.333";
            System.Net.IPAddress returnIP;
            var isValid = PortScanning.TryParse(ip, out returnIP);

            Assert.IsFalse(isValid);
        }

        [TestMethod]
        public void TryParseTest3()
        {
            string ip = "123.3.2";
            System.Net.IPAddress returnIP;
            var isValid = PortScanning.TryParse(ip, out returnIP);

            Assert.IsFalse(isValid);
        }

        [TestMethod]
        public void ValidateIPsTest1()
        {
            string beginIP = null;
            string endIP = null;

            Assert.ThrowsException<ArgumentException>(() => Validation.ValidateIPs(beginIP, endIP));
        }

        [TestMethod]
        public void ValidateIPsTest2()
        {
            string beginIP = null;
            string endIP = "127.0.0.1";

            Assert.ThrowsException<ArgumentException>(() => Validation.ValidateIPs(beginIP, endIP));
        }

        [TestMethod]
        public void ValidateIPsTest3()
        {
            string beginIP = "127.0.0.10";
            string endIP = "127.0.0.1";

            var isValid = Validation.ValidateIPs(beginIP, endIP);
        }

        [TestMethod]
        public void ValidateIPsTest4()
        {
            string beginIP = "127.0.0";
            string endIP = "127.0.0.1";

            Assert.ThrowsException<ArgumentException>(() => Validation.ValidateIPs(beginIP, endIP));
        }
        [TestMethod]
        public void ValidateIPsTest5()
        {
            string beginIP = "127.0.0.1";
            string endIP = "127.0.0.10";
            var isValid = Validation.ValidateIPs(beginIP, endIP);

            Assert.IsTrue(isValid);
        }

        [TestMethod]
        public void GetRangeTest1()
        {
            string beginIPStr = "127.0.0.10";
            string endIPStr = "127.0.0.1";

            IPAddress beginIP;
            IPAddress endIP;
            var isBeginIPValid = PortScanning.TryParse(beginIPStr, out beginIP);
            var isEndIPValid = PortScanning.TryParse(endIPStr, out endIP);

            Assert.ThrowsException<ArgumentException>(() => PortScanning.GetIPRange(beginIP, endIP));
        }

        [TestMethod]
        public void GetRangeTest2()
        {
            string beginIPStr = "127.0.0.1";
            string endIPStr = "127.0.0.10";

            IPAddress beginIP;
            IPAddress endIP;
            var isBeginIPValid = PortScanning.TryParse(beginIPStr, out beginIP);
            var isEndIPValid = PortScanning.TryParse(endIPStr, out endIP);

            var range = PortScanning.GetIPRange(beginIP, endIP);

            Assert.AreEqual(range.Count, 10);
        }

        [TestMethod]
        public void GetRangeTest3()
        {
            string beginIPStr = "127.0.0.1";
            string endIPStr = "127.0.0.10";

            IPAddress beginIP;
            IPAddress endIP;
            var isBeginIPValid = PortScanning.TryParse(beginIPStr, out beginIP);
            var isEndIPValid = PortScanning.TryParse(endIPStr, out endIP);

            var range = PortScanning.GetIPRange(beginIP, endIP);

            Assert.IsTrue(range.Contains("127.0.0.7"));
            Assert.IsFalse(range.Contains("127.0.0.15"));
        }
    }
}
