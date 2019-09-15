using PortScanner.Contracts;
using System;
using System.Windows.Input;

namespace PortScanner.Commands
{
    public class CancelScanningCommand : ICommand
    {
        private bool _executable;

        public event EventHandler CanExecuteChanged;
        private IViewModel _viewModel { get; set; }
        public bool Executable {
            get
            {
                return _executable;
            }
            set
            {
                _executable = value;
            }
        }

        public CancelScanningCommand(IViewModel viewModel)
        {
            _viewModel = viewModel;
        }
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _viewModel.CancelScanning(parameter);
        }
    }
}