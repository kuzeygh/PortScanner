using PortScanner.Commands;
using PortScanner.Contracts;
using PortScanner.Enums;
using PortScanner.Models;
using PortScanner.Utility;
using System;
using System.Collections.Async;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace PortScanner.ViewModels
{
    public class MainViewModel : ObservableCollection<PortModel>, IViewModel
    {
        private ObservableCollection<PortModel> _openPorts;

        public bool IsScannable
        {
            get
            {
                return _isScannable;
            }

            set
            {
                _isScannable = value;
            }
        }

        public bool IsCancellable
        {
            get
            {
                return !_isScannable;
            }
        }
        public ObservableCollection<PortModel> OpenPorts
        {
            get
            {
                return _openPorts;
            }

            set
            {
                _openPorts = value;
            }
        }

        public double MaxThreadsCount
        {
            get
            {
                return _maxThreadsCount;
            }

            set
            {
                _maxThreadsCount = value;
            }
        }
        private double _maxThreadsCount;
        private CancellationTokenSource source = null;
        private IPAddress _beginIP;
        private IPAddress _endIP;
        private ILogger _logger;
        private string _beginIPStr;
        private string _endIPStr;
        private bool _isScannable;

        public string BeginIP {
            get
            {
                return _beginIPStr;
            }

            set
            {
                _beginIPStr = value;
            }
        }
        public string EndIP {
            get
            {
                return _endIPStr;
            }

            set
            {
                _endIPStr = value;
            }
        }

        public ScanPortsCommand ScanPortsCommand { get; set; }
        public CancelScanningCommand CancelScanningCommand { get; set; }
       
        public MainViewModel()
        {
            this.ScanPortsCommand = new ScanPortsCommand(this);
            this.CancelScanningCommand = new CancelScanningCommand(this);
            _openPorts = new ObservableCollection<PortModel>();
            _logger = new FileLogger();
            PortScanning.Logger = _logger;

            SetButtonsEnable(true);
        }

        private void SetButtonsEnable(bool isScannable)
        {
            _isScannable = isScannable;
        }

        private bool ValidateIPs(string beginIPStr, string endIPStr)
        {
            try
            {
                Validation.ValidateIPs(beginIPStr, endIPStr);
                return true;
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Warning");
                return false;
            }
        }

        public void CancelScanning(object parameter)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to cancel scanning?", "Warning", MessageBoxButton.OKCancel);

            if (result == MessageBoxResult.OK)
            {
                if (source != null)
                    source.Cancel();
                CancelScanningCommand.Executable = false;
            }
        }

        public async void ScanPortsAsync(object parameter)
        {
            try
            {
                if (ValidateIPs(_beginIPStr, _endIPStr))
                {
                    SetButtonsEnable(false);

                    List<string> range = new List<string>();

                    PortScanning.TryParse(_beginIPStr, out _beginIP);
                    PortScanning.TryParse(_endIPStr, out _endIP);

                    bool _isSameIP = _beginIPStr == _endIPStr;

                    if (_isSameIP)
                    {
                        _endIPStr = _beginIPStr;
                        range = new List<string> { _beginIPStr };
                    }
                    else
                    {
                        try
                        {
                            range = PortScanning.GetIPRange(_beginIP, _endIP);
                        }
                        catch (ArgumentException ex)
                        {
                            MessageBox.Show(ex.Message, "Warning");
                            return;
                        }
                    }
                    //_openPorts = new ObservableCollection<PortModel>();
                    //dgOpenedPorts.ItemsSource = _openedPorts;

                    await ScanPorts(range, _openPorts, (int)_maxThreadsCount);
                }
                else
                    return;
            }
            catch (OperationCanceledException)
            {
                await _logger.LogAsync("Scanning was canceled", LogSeverity.Info);
                SetButtonsEnable(true);
                source = null;
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
                await _logger.LogAsync(ex.ToString(), LogSeverity.Error);
                SetButtonsEnable(true);
                source = null;
                return;
            }
        }

        private async Task ScanPorts(List<string> range, ObservableCollection<PortModel> openedPorts, int maxThreads)
        {
            var allTasks = new List<Task>();
            var throttler = new SemaphoreSlim(initialCount: maxThreads);

            source = new CancellationTokenSource();

            foreach (var ip in range)
            {
                await throttler.WaitAsync(source.Token);
                allTasks.Add(
                    Task.Run(async () =>
                    {
                        try
                        {
                            await PortScanning.PortDetecter(ip, openedPorts, source.Token);
                        }
                        finally
                        {
                            throttler.Release();
                        }
                    }));
            }
            await Task.WhenAll(allTasks);
        }
    }
}
