using System.Net.Sockets;
using ShDeviceContext;
using ShPackages;

namespace DeviceServer
{
    public class SwitcherDevice : BaseDevice
    {
        private class SwitchCommand : BaseDeviceCommand
        {
            private readonly SwitcherDevice mDevice;
            public override PackageType CommandType => PackageType.Switch;
            public override string Name => "Переключить";

            protected override void SendPackage(Package pack)
            {
                mDevice.Send(pack);
            }

            public SwitchCommand(SwitcherDevice device)
            {
                mDevice = device;
            }
        }

        public SwitcherDevice(Socket socket, Device deviceRecord)
            : base(socket, deviceRecord)
        {
            CommandsInner.Add(new SwitchCommand(this));
        }

        public void Switch()
        {
            Send(new Package(PackageType.Switch, null));
        }

        public override void ProcessPackage(Package package)
        {
        }
    }
}