using KaraWeb.Core.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Threading.Tasks;

namespace KaraWeb.Host
{
    public class EntryPoint
    {
        public static async Task Main(string[] args)
        {
            var server = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<Startup>()
                .UseUrls(Constants.Uri);
            });
            await server.RunConsoleAsync();
        }
    }
}