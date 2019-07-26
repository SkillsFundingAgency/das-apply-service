using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Configuration;
using Microsoft.Extensions.Configuration;

namespace SFA.DAS.ApplyService.Web.IntegrationTests
{
    [TestFixture]
    public class WhenCallingSignInPage
    {
        private HttpClient _client;

        [SetUp]
        public void Setup()
        {
            var configurationService = new Mock<IConfigurationService>();

            configurationService.Setup(c => c.GetConfig())
                .ReturnsAsync(new ApplyConfig() {SessionRedisConnectionString = "HelloDave"});

            var configuration = new Mock<IConfiguration>();
            configuration.SetupGet(x => x["EnvironmentName"]).Returns("LOCAL");
            configuration.SetupGet(x => x["ConfigurationStorageConnectionString"]).Returns("UseDevelopmentStorage=true;");

            var httpContextAccessor = new Mock<IHttpContextAccessor>();

            var builder = new WebHostBuilder();
            builder.ConfigureServices(services =>
            {
                services.AddSingleton(p => configurationService.Object);
                services.AddSingleton(p => configuration.Object);
                services.AddTransient(p => httpContextAccessor.Object);
            });
            builder.UseStartup<Startup>();

            var testServer = new TestServer(builder);
            
            _client = testServer.CreateClient();
        }
        
        [Test]
        public async Task ThenUserIsRedirectedToDfeSigninPage()
        {
            var response = await _client.GetAsync("/Users/SignIn");
            response.StatusCode.Should().Be(HttpStatusCode.Redirect);
        }
    }
}