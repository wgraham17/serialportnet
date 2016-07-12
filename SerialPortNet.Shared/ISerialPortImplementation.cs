namespace SerialPortNet
{
    using System.Threading.Tasks;

    internal interface ISerialPortImplementation
    {
        bool IsOpen();

        void Open();

        Task<byte[]> ReadAsync();

        Task WriteAsync(byte[] data);
    }
}
