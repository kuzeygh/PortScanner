using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortScanner
{
    public static class Ports
    {
        public static ObservableCollection<string> OpenPorts { get; set; }
        //private static Ports _instance = new Ports();
        //private static Object lockObj = new Object();
        //private Ports()
        //{

        //}

        //public static Ports Instance()
        //{
        //    if (_instance == null)
        //    {
        //        lock (lockObj)
        //        {
        //            if (_instance == null)
        //                _instance = new Ports();
        //        }
        //    }
        //    return _instance;

        //}
    }
}
