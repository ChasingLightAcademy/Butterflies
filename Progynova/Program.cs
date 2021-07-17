using System;
using System.Net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Web;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace Progynova
{
    public class Program
    {
        #region Program Info

        public static readonly string Name = "Progynova";
        public static readonly string Author = "Qyl";
        public static readonly Version Version = new(1, 0, 0, 0);

        #endregion

        public static void Main(string[] args)
        {
            var logger = LogManager.GetCurrentClassLogger();
            logger.Info($"Starting {Name} Ver: {Version} by {Author}.");
            
            CreateHostBuilder(args).Build().Run();
            
            LogManager.Shutdown();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>()
                        .UseKestrel(kestrel =>
                        {
                            var config = (IConfiguration)kestrel.ApplicationServices.GetService(typeof(IConfiguration));
                            if (config == null)
                            {
                                return;
                            }

                            foreach (var listener in config.GetSection("Listeners").GetChildren())
                            {
                                if (bool.TryParse(listener["Enable"], out var isEnable) && isEnable)
                                {
                                    if (int.TryParse(listener["Port"], out var port))
                                    {
                                        kestrel.Listen(IPAddress.Any, port, option =>
                                        {
                                            option.UseConnectionLogging();
                                            if (!string.IsNullOrWhiteSpace(listener["Cert"]) &&
                                                !string.IsNullOrWhiteSpace(listener["Password"]))
                                            {
                                                option.UseHttps(listener["Cert"], listener["Password"]);
                                            }
                                        });
                                    }
                                }
                            }
                        })
                        .ConfigureLogging(logging =>
                        {
                            logging.ClearProviders();
                            logging.SetMinimumLevel(LogLevel.Information);
                        })
                        .UseNLog();
                });
    }
}