namespace SerialPortNet
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class ManagedSerialPort : IDisposable
    {
        public event EventHandler<ConnectionStatusChangedEventArgs> ConnectionStatusChanged;
        public event EventHandler<MessageReceivedEventArgs> MessageReceived;

        private bool isInitialized;
        private CancellationTokenSource tokenSource;
        private ISerialPortImplementation serialPortImplementation;

        internal ManagedSerialPort(ISerialPortImplementation serialPortImplementation)
        {
            this.tokenSource = new CancellationTokenSource();
            this.serialPortImplementation = serialPortImplementation;
        }

        public bool IsConnected
        {
            get
            {
                return this.serialPortImplementation.IsOpen();
            }
        }

        public void Initialize()
        {
            if (!this.isInitialized)
            {
                this.isInitialized = true;
                this.serialPortImplementation.Open();

                this.ConnectionStatusChanged?.Invoke(this, new ConnectionStatusChangedEventArgs(true));

                Task.Factory.StartNew(() => this.ReadPump(this.tokenSource.Token), TaskCreationOptions.LongRunning);
                Task.Factory.StartNew(() => this.ConnectionMonitorPump(this.tokenSource.Token), TaskCreationOptions.LongRunning);
            }
        }

        public void Write(byte[] data)
        {
            AsyncHelper.RunSync(() => this.serialPortImplementation.WriteAsync(data));
        }

        public void Dispose()
        {
            this.tokenSource.Dispose();
            this.serialPortImplementation.Dispose();
        }

        private async Task ConnectionMonitorPump(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                if (!this.serialPortImplementation.IsOpen())
                {
                    this.ConnectionStatusChanged?.Invoke(this, new ConnectionStatusChangedEventArgs(false));

                    this.serialPortImplementation.Open();
                    this.ConnectionStatusChanged?.Invoke(this, new ConnectionStatusChangedEventArgs(true));
                }

                await Task.Delay(500);
            }
        }

        private async Task ReadPump(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                if (this.serialPortImplementation.IsOpen())
                {
                    var data = await this.serialPortImplementation.ReadAsync();

                    if (data?.Length > 0)
                    {
                        this.MessageReceived?.Invoke(this, new MessageReceivedEventArgs(data));
                    }
                }
                else
                {
                    await Task.Delay(100);
                }
            }
        }
    }
}
