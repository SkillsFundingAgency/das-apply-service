using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.ApplyService.Configuration;

namespace SFA.DAS.ApplyService.InternalApi.StartupExtensions
{
    public static class CacheStartupExtensions
    {
        public static IServiceCollection AddCache(this IServiceCollection services, IApplyConfig configuration, IHostingEnvironment environment)
        {
            if (environment.IsDevelopment())
            {
                services.AddDistributedMemoryCache();
            }
            else
            {
                services.AddStackExchangeRedisCache(options =>
                {
                    var redisConnectionString = configuration.SessionRedisConnectionString;
                    var sessionCachingDatabase = configuration.SessionCachingDatabase;
                    options.Configuration = $"{redisConnectionString},{sessionCachingDatabase}";
                });
            }

            return services;
        }
    }
}
