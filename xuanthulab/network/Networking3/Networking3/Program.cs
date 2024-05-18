using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Networking3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine(HttpListener.IsSupported);

            var server = new HttpListener();
            server.Start();
        }
    }
}
