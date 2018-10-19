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
using SFA.DAS.ApplyService.Web.IntegrationTests.Infrastructure;

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

            var httpContextAccessor = new Mock<IHttpContextAccessor>();
            
            var builder = new WebHostBuilder().UseStartup<FakeStartup>();
            builder.ConfigureServices(services =>
            {
                services.AddSingleton(p => configurationService.Object);
                services.AddTransient(p => httpContextAccessor.Object);
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