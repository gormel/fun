using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RuRaApiTest
{
    class RuRaCommunicator
    {
        const string RootUrl = "http://ruranobe.ru";
        readonly HttpClient mClient = new HttpClient();

        public async Task Login()
        {
            var result = await mClient.PostAsync($"{RootUrl}/api/users", 
                new StringContent("{ \"username\": \"gremkil\", \"password\": \"2147483647\" }", Encoding.UTF8, "application/json"));
            var loginResult = await result.Content.ReadAsStringAsync();
            result = await mClient.GetAsync($@"{RootUrl}/api/projects/9/subprojects?show_hidden=true");
            var stringResult = await result.Content.ReadAsStringAsync();
            Console.WriteLine(loginResult);
            Console.WriteLine("--------");
            Console.WriteLine(stringResult);
        }
    }
}
