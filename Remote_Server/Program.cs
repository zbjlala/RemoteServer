using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Server_Core.Tcp;
using Server_Helper;
using System;

namespace Remote_Server
{
    public class Program
    {
        private static ServerSocket server;

        public static void Main(string[] args)
        {
            //RemoteServerStart();
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
      
    }
}
