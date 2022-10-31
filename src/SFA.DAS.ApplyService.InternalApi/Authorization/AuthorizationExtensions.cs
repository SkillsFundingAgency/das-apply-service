using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace SFA.DAS.ApplyService.InternalApi.Authorization
{
    public static class AuthorizationExtensions
    {
        public static IServiceCollection AddApiAuthorization(this IServiceCollection services, IWebHostEnvironment environment)
        {
            var isDevelopment = environment.IsDevelopment();

            services.AddAuthorization(x =>
            {
                x.AddPolicy("Default", policy =>
                {
                    if (isDevelopment)
                    {
                        policy.AllowAnonymousUser();
                    }
                    else
                    {
                        policy.RequireAuthenticatedUser();
                        policy.RequireRole("ApplyServiceInternalAPI");
                    }
                });

                x.DefaultPolicy = x.GetPolicy("Default");
            });

            if (isDevelopment)
            {
                services.AddSingleton<IAuthorizationHandler, LocalAuthorizationHandler>();
            }

            return services;
        }
    }
}
