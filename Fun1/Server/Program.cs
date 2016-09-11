using System.Diagnostics;
using SuperCore.Core;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            var server = new SuperServer();

            var listen = server.Listen(666);
        }
    }
}
