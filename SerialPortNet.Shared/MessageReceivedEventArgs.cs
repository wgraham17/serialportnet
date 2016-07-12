namespace SerialPortNet
{
    using System;

    public class MessageReceivedEventArgs : EventArgs
    {
        internal MessageReceivedEventArgs(byte[] data)
        {
            this.Data = data;
        }

        public byte[] Data { get; private set; }
    }
}
