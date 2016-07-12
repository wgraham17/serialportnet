namespace SerialPortNet
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class ManagedSerialPort : IDisposable
    {
        public event EventHandler<MessageReceivedEventArgs> MessageReceived;

        private bool isInitialized;
        private CancellationTokenSource tokenSource;
        private ISerialPortImplementation serialPortImplementation;

        internal ManagedSerialPort(ISerialPortImplementation serialPortImplementation)
        {
            this.tokenSource = new CancellationTokenSource();
            this.serialPortImplementation = serialPortImplementation;
        }

        public void Initialize()
        {
            if (!this.isInitialized)
            {
                this.isInitialized = true;
                this.serialPortImplementation.Open();

                Task.Factory.StartNew(() => this.ReadPump(this.tokenSource.Token), TaskCreationOptions.LongRunning);
                Task.Factory.StartNew(() => this.ConnectionMonitorPump(this.tokenSource.Token), TaskCreationOptions.LongRunning);
            }
        }

        public async Task Write(byte[] data)
        {
            await this.serialPortImplementation.WriteAsync(data);
        }

        public void Dispose()
        {
            this.tokenSource.Dispose();
        }

        private async Task ConnectionMonitorPump(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                if (!this.serialPortImplementation.IsOpen())
                {
                    this.serialPortImplementation.Open();
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
