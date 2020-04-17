using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;

namespace Store.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            IConfiguration configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("url.json").Build();

          return  Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {//.UseUrls(configuration["url"])
                    webBuilder .UseStartup<Startup>();
                })
            .ConfigureLogging(logging => //≈‰÷√Nlog
            {
                logging.ClearProviders();
                logging.AddConsole();
                logging.SetMinimumLevel( LogLevel.Trace);
                
            }).UseNLog();
        }
    }
}
