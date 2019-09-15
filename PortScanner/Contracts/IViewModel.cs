namespace PortScanner.Contracts
{
    public interface IViewModel
    {
        void CancelScanning(object parameter);
        void ScanPortsAsync(object parameter);
    }
}