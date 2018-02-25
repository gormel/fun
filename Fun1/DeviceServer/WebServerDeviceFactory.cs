using System.Net.Sockets;
using ShDeviceContext;

namespace DeviceServer
{
    public class WebServerDeviceFactory : IDeviceFactory
    {
        public IDevice Create(Socket socket, Device deviceRecord)
        {
            return new WebServerDevice(socket, deviceRecord);
        }
    }
}