using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ShDeviceContext;
using ShPackages;

namespace DeviceServer
{
    class Program
    {
        private static readonly ShDeviceDataContext mDatabase = new ShDeviceDataContext();
        private static readonly Dictionary<DeviceType, IDeviceFactory> mDeviceFactorys = new Dictionary<DeviceType, IDeviceFactory>
        {
            { DeviceType.Switcher, new SwitcherDeviceFactory() },
            { DeviceType.WebServer, new WebServerDeviceFactory() },
        };

        public static List<IDevice> Devices { get; } = new List<IDevice>();

        public static IDevice CreateDevice(Socket clientSock)
        {
            var endPoint = (IPEndPoint) clientSock.RemoteEndPoint;
            var mac = string.Join("-", GetMacAddress(endPoint.Address).Select(b => b.ToString("X")));
            var deviceRecord = mDatabase.Devices.FirstOrDefault(d => d.Mac == mac);
            if (deviceRecord == null)
                return null;

            IDeviceFactory factory;
            if (!mDeviceFactorys.TryGetValue((DeviceType)deviceRecord.Type, out factory))
                return null;
            var result = factory.Create(clientSock, deviceRecord);
            Devices.Add(result);
            return result;
        }

        public static IEnumerable<T> GetDevices<T>() where T : IDevice
        {
            return Devices.OfType<T>();
        }

        static void Main(string[] args)
        {
            var serverSock = new Socket(SocketType.Stream, ProtocolType.Tcp);
            var source = new CancellationTokenSource();
            try
            {
                serverSock.Listen(15);
                var t = LestenAsync(serverSock, source.Token);
                string cmd;
                while ((cmd = Console.ReadLine()) != "stop")
                {
                    Thread.Sleep(200);
                }
            }
            finally
            {
                source.Cancel();
                serverSock.Close();
            }
                
        }

        private static async Task LestenAsync(Socket serverSocket, CancellationToken token = default(CancellationToken))
        {
            while (true)
            {
                token.ThrowIfCancellationRequested();
                var clientSock = await Task.Factory.FromAsync(serverSocket.BeginAccept, serverSocket.EndAccept, null);
                token.ThrowIfCancellationRequested();
#pragma warning disable 4014
                ProcessClientAsync(clientSock, token);
#pragma warning restore 4014
            }
        }

        [DllImport("iphlpapi.dll", ExactSpelling = true)]
        public static extern int SendARP(uint destIp, uint srcIp, byte[] macAddress, ref uint macAddressLength);

        public static byte[] GetMacAddress(IPAddress address)
        {
            byte[] mac = new byte[6];
            uint len = (uint)mac.Length;
            byte[] addressBytes = address.GetAddressBytes();
            uint dest = ((uint)addressBytes[3] << 24)
              + ((uint)addressBytes[2] << 16)
              + ((uint)addressBytes[1] << 8)
              + ((uint)addressBytes[0]);
            if (SendARP(dest, 0, mac, ref len) != 0)
            {
                throw new Exception("The ARP request failed.");
            }
            return mac;
        }

        private static async Task ProcessClientAsync(Socket clientSocket, CancellationToken token = default(CancellationToken))
        {
            using (var device = CreateDevice(clientSocket))
            {
                while (true)
                {
                    var package = await Package.Read(device.DeviceDataStream);
                    if (package == null)
                        continue;
                    device.ProcessPackage(package);
                }
            }
        }
    }
}
