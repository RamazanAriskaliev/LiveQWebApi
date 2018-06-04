using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LiveQ.Api.Data;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace LiveQ.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();
            SeedData(host);
            host.Run();
        }

        private static void SeedData(IWebHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    // Requires using RazorPagesMovie.Models;
                    DataInitializer.Seed(services).Wait();
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred seeding the DB.");
                }
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args).UseStartup<Startup>();
                // .UseKestrel()
                // .UseContentRoot(Directory.GetCurrentDirectory())
                // .UseIISIntegration()
                // .UseStartup<Startup>()
                // .ConfigureAppConfiguration((hostContext, config) =>
                // {
                //     // delete all default configuration providers
                //     config.Sources.Clear();
                //     config.AddJsonFile("appsettings.json", optional: true);
                //     config.AddEnvironmentVariables();
                // })
                // .UseApplicationInsights()
                
    }
}
