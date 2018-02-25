using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ShPackages
{
    public class Package
    {
        [JsonProperty("type")]
        public PackageType Type { get; private set; }
        [JsonProperty("data")]
        public IPackageData Data { get; private set; }

        protected Package()
        {
            
        }

        public Package(PackageType type, IPackageData data)
        {
            Type = type;
            Data = data;
        }

        public static async Task<Package> Read(Stream stream)
        {
            var len = BitConverter.ToInt32(await ReadBytes(stream, 4), 0);
            var body = Encoding.UTF8.GetString(await ReadBytes(stream, len));
            return JsonConvert.DeserializeObject<Package>(body);
        }

        private static async Task<byte[]> ReadBytes(Stream stream, int bytec)
        {
            var read = 0;
            byte[] result = new byte[bytec];
            while (read < bytec)
            {
                read += await stream.ReadAsync(result, read, bytec - read);
            }
            return result;
        }

        public static async Task Write(Stream stream, Package pack)
        {
            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(pack));
            var len = BitConverter.GetBytes(body.Length);
            await stream.WriteAsync(len, 0, len.Length);
            await stream.WriteAsync(body, 0, body.Length);
        }
    }
}
