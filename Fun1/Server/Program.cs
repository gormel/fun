using System;
using System.Diagnostics;
using Interfaces;
using SuperCore.Core;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            var server = new SuperServer();

            server.Register<ILoginServer>(new LoginServer());

            var listen = server.Listen(666);

            while (Console.ReadLine()?.ToLower() != "exit")
            {
                
            }
        }
    }
}
