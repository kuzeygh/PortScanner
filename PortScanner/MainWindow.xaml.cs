using PortScanner.Contracts;
using PortScanner.Enums;
using PortScanner.Models;
using PortScanner.Utility;
using PortScanner.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace PortScanner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<PortModel> OpenedPorts
        {
            get
            {
                return _openedPorts;
            }
            set
            {
                _openedPorts = value;
            }
        }
        private CancellationTokenSource source = null;
        private IPAddress _beginIP;
        private IPAddress _endIP;
        private ObservableCollection<PortModel> _openedPorts;
        private ILogger _logger;

        public MainWindow()
        {
            InitializeComponent();
            _logger = new FileLogger();
            PortScanning.Logger = _logger;
            this.DataContext = new MainViewModel();
            
            //SetButtonsEnable(true);
        }

       
    }
}
