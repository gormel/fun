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
                    var pack = new Package(PackageType.DeviceList);
                    var devices = new CustomObject[Program.Devices.Count];
                    pack.Data.Add("devices", devices);
                    int i = 0;
                    foreach (var device in Program.Devices)
                    {
                        var deviceDescription = new CustomObject();
                        deviceDescription["name"] = device.Name;
                        deviceDescription["type"] = (int) device.Type;
                        deviceDescription["tag"] = device.Tag;

                        var commands = new CustomObject[device.Commands.Count];
                        deviceDescription["commands"] = commands;
                        int j = 0;
                        foreach (var command in device.Commands)
                        {
                            var commandDescription = new CustomObject();
                            commandDescription["name"] = command.Name;
                            commandDescription["type"] = (int) command.CommandType;
                            commands[j++] = commandDescription;
                        }

                        devices[i++] = deviceDescription;
                    }
                    Send(new Package(PackageType.DeviceList));
                    break;
            }
        }
    }
}