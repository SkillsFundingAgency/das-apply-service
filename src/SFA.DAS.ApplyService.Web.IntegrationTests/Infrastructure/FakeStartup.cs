using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.ApplyService.IntegrationTestHelpers;

namespace SFA.DAS.ApplyService.Web.IntegrationTests.Infrastructure
{
    public class FakeStartup : Startup
    {
        public FakeStartup(IConfiguration configuration) : base(configuration)
        {
        }

        protected override void ConfigureAuth(IServiceCollection services)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "Test Scheme";
                options.DefaultChallengeScheme = "Test Scheme";
            }).AddTestAuth(o => { });
        }

    }
}