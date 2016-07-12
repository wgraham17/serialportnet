#if WINDOWS_UWP

namespace SerialPortNet
{
    using System;
    using System.Runtime.InteropServices.WindowsRuntime;
    using System.Threading.Tasks;
    using Windows.Devices.SerialCommunication;

    internal class UWPSerialPortImplementation : ISerialPortImplementation
    {
        private SerialDevice serialDevice;

        public UWPSerialPortImplementation(SerialDevice serialDevice, SerialPortOptions options)
        {
            this.serialDevice = serialDevice;
            this.serialDevice.BaudRate = options.BaudRate;
            this.serialDevice.Parity = options.Parity.ToSerialParity();
            this.serialDevice.StopBits = options.StopBits.ToSerialStopBitCount();
            this.serialDevice.ReadTimeout = TimeSpan.FromMilliseconds(options.ReadTimeoutMs);
            this.serialDevice.WriteTimeout = TimeSpan.FromMilliseconds(options.WriteTimeoutMs);
        }

        public void Open()
        {
        }

        public bool IsOpen()
        {
            return true;
        }

        public async Task<byte[]> ReadAsync()
        {
            var buffer = new Windows.Storage.Streams.Buffer(1024);
            var bytesRead = await this.serialDevice.InputStream.ReadAsync(buffer, buffer.Capacity, Windows.Storage.Streams.InputStreamOptions.None);

            return buffer.ToArray();
        }

        public async Task WriteAsync(byte[] data)
        {
            await this.serialDevice.OutputStream.WriteAsync(data.AsBuffer());
        }

        public void Dispose()
        {
            this.serialDevice.Dispose();
        }
    }
}

#endif
