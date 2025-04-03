using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace SFA.DAS.ApplyService.Web
{
    public static class Program
    {
        public static void Main(string[] args)
        {

        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseUrls("https://localhost:6016", "http://localhost:6015");
    }
}