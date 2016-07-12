namespace SerialPortNet
{
    using System;
    using System.Threading.Tasks;

#if WINDOWS_UWP
    using Windows.Devices.Enumeration;
    using Windows.Devices.SerialCommunication;
#endif

    public class SerialPortFactory
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
#endif
    }
}
