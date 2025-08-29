using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using NLog.Extensions.Logging;

namespace FHT.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args).ConfigureLogging((hostingContext, logging) =>
                {
                    logging.AddNLog(hostingContext.Configuration.GetSection("Logging"));
                })
                .UseStartup<Startup>();
    }
}
