using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Configuration;

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


            var builder = new WebHostBuilder().UseStartup<Startup>();
            builder.ConfigureServices(services =>
            {
                services.AddSingleton<IConfigurationService>(p => configurationService.Object);
            });

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