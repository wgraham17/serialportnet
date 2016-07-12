namespace SerialPortNet
{
    using System;
    using System.Threading.Tasks;

    internal interface ISerialPortImplementation : IDisposable
    {
        bool IsOpen();

        void Open();

        Task<byte[]> ReadAsync();

        Task WriteAsync(byte[] data);
    }
}
