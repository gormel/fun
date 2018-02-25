using System.Net.Sockets;
using ShDeviceContext;

namespace DeviceServer
{
    public class SwitcherDeviceFactory : IDeviceFactory
    {
        public IDevice Create(Socket socket, Device deviceRecord)
        {
            return new SwitcherDevice(socket, deviceRecord);
        }
    }
}