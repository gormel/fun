using System;
using System.Collections.Generic;
using System.IO;
using ShPackages;

namespace DeviceServer
{
    public interface IDevice : IDisposable
    {
        void ProcessPackage(Package package);
        DeviceType Type { get; }
        string Name { get; }
        string Tag { get; }
        Stream DeviceDataStream { get; }
        IReadOnlyCollection<IDeviceCommand> Commands { get; } 
    }
}