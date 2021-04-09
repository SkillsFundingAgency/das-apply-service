using System;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.DependencyInjection;

namespace SFA.DAS.ApplyService.Web.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddStubAuthe(this IServiceCollection services, Action<CookieAuthenticationOptions> cookieAuthenticationOptions, OpenIdConnectEvents events = null)
        {
            services.AddAuthentication(options =>
                {
                    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
                })
                .AddCookie(cookieAuthenticationOptions)
                .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
                {
                    options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.Authority = "https://localhost:44381/";
                    options.ClientId = "openIdConnectRoATPClient";
                    options.Scope.Add("openid");
                    options.Scope.Add("roatp-apply");
                    options.ResponseType = "id_token";
                    options.UseTokenLifetime = false;
                    options.RequireHttpsMetadata = false;
                    options.Events = events;
                });

            return services;
        }
    }

    public static class IdentityConstants
    {
        //public const string Subject = "sub";
        public const string Subject = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";
    }
}
