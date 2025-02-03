using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartFactorySample.DataReception.WebUI
{
    public class Program
    {
        private static string _env = "Dev";

        public static void Main(string[] args)
        {
            SetEnvironment();

            CreateHostBuilder(args).Build().Run();
        }
        private static void SetEnvironment()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false)
                .Build();
            _env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        }
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((appConfiguration) =>
            {
                appConfiguration.AddJsonFile($"appsettings.json", optional: false, reloadOnChange: true);
                appConfiguration.AddJsonFile($"appsettings.{_env}.json", optional: false, reloadOnChange: true);
            })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
