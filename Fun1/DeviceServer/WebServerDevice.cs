using System.Linq;
using System.Net.Sockets;
using ShDeviceContext;
using ShPackages;

namespace DeviceServer
{
    public class WebServerDevice : BaseDevice
    {
        public WebServerDevice(Socket socket, Device deviceRecord)
            : base(socket, deviceRecord)
        {
        }

        public override void ProcessPackage(Package package)
        {
            switch (package.Type)
            {
                case PackageType.DeviceList:
                    Send(new Package(
                        PackageType.DeviceList, 
                        new DeviceListPackageData(
                            Program.Devices.Select(d => new DeviceListEntity(
                                d.Name, 
                                d.Tag, 
                                d.Commands.Select(c => new PossibleActionEntity(c.CommandType, c.Name)).ToArray())).ToArray()
                        )));
                    break;
            }
        }
    }
}