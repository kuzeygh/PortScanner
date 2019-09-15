using PortScanner.Contracts;
using PortScanner.Enums;
using PortScanner.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace PortScanner
{
    public static class PortScanning
    {
        private static readonly Regex _ipRegex = new Regex(@"((25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)");

        public static bool TryParse(string IPAdressStr, out IPAddress IPAddress)
        {
            IPAddress = null;

            try
            {
                if (!_ipRegex.IsMatch(IPAdressStr))
                    return false;

                var addressStrArr = IPAdressStr.Split('.');
                byte[] addressArr = new byte[4];

                if (addressStrArr.Length == 4)
                    for (var i = 0; i < 4; i++)
                        addressArr[3 - i] = byte.Parse(addressStrArr[i]);

                IPAddress = new IPAddress(addressArr);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static List<string> GetIPRange(IPAddress startIP, IPAddress endIP)
        {
            if (startIP.AddressFamily != endIP.AddressFamily)
                throw new ArgumentException("Elements must be of the same address family", nameof(endIP));

            byte[] bytes;
            int start = BitConverter.ToInt32(startIP.GetAddressBytes(), 0);
            int end = BitConverter.ToInt32(endIP.GetAddressBytes(), 0);

            if(start > end)
                throw new ArgumentException("Begin IP must be bigger than second");

            List<string> addresses = new List<string>();
            for (int i = start; i <= end; i++)
            {
                bytes = BitConverter.GetBytes(i);
                addresses.Add(new IPAddress(new[] { bytes[3], bytes[2], bytes[1], bytes[0] }).ToString());
            }

            return addresses;
        }

        public static ILogger Logger { get; set; }
       
        public static async Task PortDetecter(string targetIP, ObservableCollection<PortModel> openPorts, CancellationToken ct)
        {
            List<int> openPortList = new List<int>();
            for (int i = 79; i < uint.MaxValue; i++)
            {
                if (ct.IsCancellationRequested)
                    throw new OperationCanceledException();
                using (TcpClient tcpClient = new TcpClient())
                {
                    try
                    {
                        tcpClient.Connect(targetIP, i);
                        App.Current.Dispatcher.Invoke((Action)delegate
                        {
                            openPorts.Add(new PortModel { IP = targetIP, Port = i.ToString() });
                        });
                        await Logger.LogAsync("Scanned " + targetIP + ":" + i + ", opened", LogSeverity.Info);
                    }
                    catch (Exception ex)
                    {
                        await Logger.LogAsync("Scanned " + targetIP + ":" + i + ", closed", LogSeverity.Info);
                    }
                }
            }
        }

    }
}
