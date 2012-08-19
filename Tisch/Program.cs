using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Croudia;
using Croudia.Method;

namespace Tisch
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Net.ServicePointManager.Expect100Continue = false;
            var client = new CroudiaClient(Console.ReadLine());
            Console.WriteLine(client.Excecute(new Login(client.Credential.Id, Console.ReadLine())));
            client.Excecute(new Post("abc *Croudia Library"));
        }
    }
}
