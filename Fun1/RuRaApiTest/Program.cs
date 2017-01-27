using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuRaApiTest
{
    class Program
    {


        static void Main(string[] args)
        {
            var communicator = new RuRaCommunicator();
            var t = communicator.Login();
            t.Wait();
            Console.ReadKey();
        }
    }
}
