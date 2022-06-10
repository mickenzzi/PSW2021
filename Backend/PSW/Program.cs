using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Renci.SshNet;
using System;
using System.IO;

namespace PSW
{
    public class Program
    {
        public static void Main(string[] args)
        {
          CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
