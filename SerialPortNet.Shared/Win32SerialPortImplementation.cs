#if NETFX_FULL

namespace SerialPortNet
{
    using System.IO.Ports;
    using System.Threading.Tasks;

    internal class Win32SerialPortImplementation : ISerialPortImplementation
    {
        private SerialPort serialPort;

        public Win32SerialPortImplementation(SerialPort serialPort, SerialPortOptions serialPortOptions)
        {
            this.serialPort = serialPort;
            this.serialPort.BaudRate = (int)serialPortOptions.BaudRate;
            this.serialPort.Parity = serialPortOptions.Parity.ToParity();
            this.serialPort.StopBits = serialPortOptions.StopBits.ToStopBits();
            this.serialPort.ReadTimeout = serialPortOptions.ReadTimeoutMs;
            this.serialPort.WriteTimeout = serialPortOptions.WriteTimeoutMs;
        }

        public void Open()
        {
            if (!this.serialPort.IsOpen)
            {
                this.serialPort.Open();
            }
        }

        public bool IsOpen()
        {
            return this.serialPort.IsOpen;
        }

        public async Task<byte[]> ReadAsync()
        {
            if (this.serialPort.BytesToRead > 0)
            {
                var buffer = new byte[this.serialPort.BytesToRead];
                await this.serialPort.BaseStream.ReadAsync(buffer, 0, buffer.Length);
                return buffer;
            }
            
            return null;
        }

        public async Task WriteAsync(byte[] data)
        {
            await this.serialPort.BaseStream.WriteAsync(data, 0, data.Length);
        }

        public void Dispose()
        {
            this.serialPort.Dispose();
        }
    }
}

#endif