using ShPackages;

namespace DeviceServer
{
    public interface IDeviceCommand
    {
        PackageType CommandType { get; }
        string Name { get; }
        void Call(IPackageData arg);
    }

    public abstract class BaseDeviceCommand : IDeviceCommand
    {
        public abstract PackageType CommandType { get; }
        public abstract string Name { get; }

        protected abstract void SendPackage(Package pack);

        public void Call(IPackageData arg)
        {
            SendPackage(new Package(CommandType, arg));
        }
    }
}