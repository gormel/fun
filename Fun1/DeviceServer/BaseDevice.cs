using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using Newtonsoft.Json;
using ShDeviceContext;
using ShPackages;

namespace DeviceServer
{
    public abstract class BaseDevice : IDevice
    {
        public Stream DeviceDataStream { get; }
        protected List<IDeviceCommand> CommandsInner { get; }  = new List<IDeviceCommand>();
        public IReadOnlyCollection<IDeviceCommand> Commands => CommandsInner;
        public DeviceType Type { get; }
        public string Name { get; }
        public string Tag { get; }

        protected BaseDevice(Socket socket, Device deviceRecord)
        {
            Type = (DeviceType)deviceRecord.Type;
            Name = deviceRecord.Name;
            Tag = deviceRecord.Tag;
            DeviceDataStream = new NetworkStream(socket);
        }

        protected async void Send(Package package)
        {
            await Package.Write(DeviceDataStream, package);
        }

        public abstract void ProcessPackage(Package package);

        public void Dispose()
        {
            DeviceDataStream.Dispose();
            Program.Devices.Remove(this);
        }
    }
}