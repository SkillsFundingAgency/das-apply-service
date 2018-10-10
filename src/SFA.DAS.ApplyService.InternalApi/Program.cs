﻿using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace SFA.DAS.ApplyService.InternalApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseUrls("https://localhost:6000");
    }
}