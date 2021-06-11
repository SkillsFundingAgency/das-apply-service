using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.ApplyService.Configuration;

namespace SFA.DAS.ApplyService.InternalApi.Authentication
{
    public static class AuthenticationExtensions
    {
        public static IServiceCollection AddApiAuthentication(this IServiceCollection services, IApplyConfig _applyConfig)
        {
            var azureActiveDirectoryConfiguration = _applyConfig.AzureActiveDirectoryConfiguration;

            services.AddAuthentication(auth =>
            {
                auth.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(auth =>
            {
                auth.Authority = $"https://login.microsoftonline.com/{azureActiveDirectoryConfiguration.Tenant}";
                auth.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidAudiences = azureActiveDirectoryConfiguration.Identifier.Split(",")
                };
            });

            services.AddSingleton<IClaimsTransformation, AzureAdScopeClaimTransformation>();
            return services;
        }
    }
}
