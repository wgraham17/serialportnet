namespace SerialPortNet
{
    using System;

    public enum SerialPortParity
    {
        None = 0,
        Odd = 1,
        Even = 2,
        Mark = 3,
        Space = 4
    }

    public static class SerialPortParityExtensions
    {
#if WINDOWS_UWP
        public static Windows.Devices.SerialCommunication.SerialParity ToSerialParity(this SerialPortParity source)
        {
            switch (source)
            {
                case SerialPortParity.None:
                    return Windows.Devices.SerialCommunication.SerialParity.None;

                case SerialPortParity.Odd:
                    return Windows.Devices.SerialCommunication.SerialParity.Odd;

                case SerialPortParity.Even:
                    return Windows.Devices.SerialCommunication.SerialParity.Even;

                case SerialPortParity.Mark:
                    return Windows.Devices.SerialCommunication.SerialParity.Mark;

                case SerialPortParity.Space:
                    return Windows.Devices.SerialCommunication.SerialParity.Space;

                default:
                    throw new ArgumentException($"Could not convert SerialPortParity of {source} to type Windows.Devices.SerialCommunitcation.SerialParity");
            }
        }
#endif

#if WIN32
        public static System.IO.Ports.Parity ToParity(this SerialPortParity source)
        {
            switch (source)
            {
                case SerialPortParity.None:
                    return System.IO.Ports.Parity.None;

                case SerialPortParity.Odd:
                    return System.IO.Ports.Parity.Odd;

                case SerialPortParity.Even:
                    return System.IO.Ports.Parity.Even;

                case SerialPortParity.Mark:
                    return System.IO.Ports.Parity.Mark;

                case SerialPortParity.Space:
                    return System.IO.Ports.Parity.Space;

                default:
                    throw new ArgumentException($"Could not convert SerialPortParity of {source} to type System.IO.Ports.Parity");
            }
        }
#endif
    }
}
