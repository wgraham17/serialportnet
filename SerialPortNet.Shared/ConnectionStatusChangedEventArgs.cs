namespace SerialPortNet
{
    using System;

    public class ConnectionStatusChangedEventArgs : EventArgs
    {
        public ConnectionStatusChangedEventArgs(bool isConnected)
        {
            this.IsConnected = isConnected;
        }

        public bool IsConnected { get; private set; }
    }
}
