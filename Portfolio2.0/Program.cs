using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Portfolio
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                 .ConfigureAppConfiguration((hostingContext, config) =>
                 {
                     var settings = config.Build();
                     string connString = settings["ConnectionStrings:AppConfig"];
                     if (connString != null)    
                        config.AddAzureAppConfiguration(settings["ConnectionStrings:AppConfig"]);
                 })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    var port = Environment.GetEnvironmentVariable("PORT");
                    if (port != null)
                    {
                        webBuilder.UseStartup<Startup>()
                        .UseUrls("http://*:" + port);
                    }
                    else
                    {
                        webBuilder.UseStartup<Startup>();
                    }
                });
        }
    }
}
