namespace ShPackages
{
    public class DeviceListPackageData : IPackageData
    {
        public DeviceListEntity[] Devices { get; }

        public DeviceListPackageData(DeviceListEntity[] devices)
        {
            Devices = devices;
        }
    }
}