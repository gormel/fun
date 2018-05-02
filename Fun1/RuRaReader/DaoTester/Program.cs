using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RuRaReader.Model;

namespace DaoTester
{
    class Program
    {
        static void Main(string[] args)
        {
            BeginTesting();
            Console.ReadKey(true);
        }

        static async Task BeginTesting()
        {
            var m = new SaveDataManager();
            var projects = await m.GetProjects();
            var volumes = await m.GetVolumes(projects[0].Id);

        }
    }
}
