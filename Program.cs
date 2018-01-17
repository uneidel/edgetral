using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
    using System;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Runtime.Loader;
    using System.Security.Cryptography.X509Certificates;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Azure.Devices.Client;
    using Microsoft.Azure.Devices.Client.Transport.Mqtt;
    

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
namespace edgerest
{
    public class Program
    {
        static int counter;

        public static void Main(string[] args)
        {
        
            var webHost = WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();

            webHost.Run();
        }       
        
    }
}
