namespace SerialPortNet
{
    using System;
    using System.Threading.Tasks;

#if WINDOWS_UWP
    using Windows.Devices.Enumeration;
    using Windows.Devices.SerialCommunication;
#endif

    public static class SerialPortFactory
    {
#if WINDOWS_UWP
        public static async Task<ManagedSerialPort> CreateForVendorProductAsync(ushort vendorId, ushort productId, SerialPortOptions options)
        {
            var serialPortSelector = SerialDevice.GetDeviceSelectorFromUsbVidPid(vendorId, productId);
            var devices = await DeviceInformation.FindAllAsync(serialPortSelector);

            if (devices.Count == 0)
            {
                throw new ArgumentException($"Unable to find serial device with VendorId 0x{vendorId:X}, ProductId 0x{productId:X}");
            }

            var serialPortDevice = await SerialDevice.FromIdAsync(devices[0].Id);
            var serialPortImpl = new UWPSerialPortImplementation(serialPortDevice, options);

            return new ManagedSerialPort(serialPortImpl);
        }

        public static async Task<ManagedSerialPort> CreateForDeviceIdAsync(string deviceId, SerialPortOptions options)
        {
            var serialPortDevice = await SerialDevice.FromIdAsync(deviceId);
            var serialPortImpl = new UWPSerialPortImplementation(serialPortDevice, options);

            return new ManagedSerialPort(serialPortImpl);
        }
#endif

#if NETFX_FULL
        public static ManagedSerialPort CreateForPort(string portName, SerialPortOptions options)
        {
            var serialPortDevice = new System.IO.Ports.SerialPort(portName);
            var serialPortImpl = new Win32SerialPortImplementation(serialPortDevice, options);

            return new ManagedSerialPort(serialPortImpl);
        }
#endif
    }
}
