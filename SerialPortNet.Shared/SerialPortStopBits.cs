namespace SerialPortNet
{
    using System;

    public enum SerialPortStopBits
    {
        None = 0,
        One = 1,
        Two = 2,
        OnePointFive = 3
    }

    public static class SerialPortStopBitsExtensions
    {
#if WINDOWS_UWP
        public static Windows.Devices.SerialCommunication.SerialStopBitCount ToSerialStopBitCount(this SerialPortStopBits source)
        {
            switch (source)
            {
                case SerialPortStopBits.One:
                    return Windows.Devices.SerialCommunication.SerialStopBitCount.One;

                case SerialPortStopBits.Two:
                    return Windows.Devices.SerialCommunication.SerialStopBitCount.Two;

                case SerialPortStopBits.OnePointFive:
                    return Windows.Devices.SerialCommunication.SerialStopBitCount.OnePointFive;
                    
                default:
                    throw new ArgumentException($"Could not convert SerialPortStopBits of {source} to type Windows.Devices.SerialCommunitcation.SerialStopBitCount");
            }
        }
#endif

#if NETFX_FULL
        public static System.IO.Ports.StopBits ToStopBits(this SerialPortStopBits source)
        {
            switch (source)
            {
                case SerialPortStopBits.None:
                    return System.IO.Ports.StopBits.None;

                case SerialPortStopBits.One:
                    return System.IO.Ports.StopBits.One;

                case SerialPortStopBits.Two:
                    return System.IO.Ports.StopBits.Two;

                case SerialPortStopBits.OnePointFive:
                    return System.IO.Ports.StopBits.OnePointFive;

                default:
                    throw new ArgumentException($"Could not convert SerialPortStopBits of {source} to type System.IO.Ports.StopBits");
            }
        }
#endif
    }
}
