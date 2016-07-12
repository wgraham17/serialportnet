namespace SerialPortNet
{
    public class SerialPortOptions
    {
        public uint BaudRate { get; set; }

        public SerialPortParity Parity { get; set; }

        public SerialPortStopBits StopBits { get; set; }

        public int ReadTimeoutMs { get; set; }

        public int WriteTimeoutMs { get; set; }
    }
}
