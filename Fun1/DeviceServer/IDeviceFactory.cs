using System.Net.Sockets;
using ShDeviceContext;

namespace DeviceServer
{
    public interface IDeviceFactory
    {
        IDevice Create(Socket socket, Device deviceRecord);
    }
}