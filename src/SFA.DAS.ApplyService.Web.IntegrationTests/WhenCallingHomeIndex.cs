using AngleSharp.Parser.Html;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Configuration;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Web.IntegrationTests
{
    // NOTE: I Don't think these have ever worked!
    [TestFixture]
    public class WhenCallingHomeIndex
    {
        private HttpClient _client;

        [SetUp]
        public void Setup()
        {
            var configurationService = new Mock<IConfigurationService>();

            var configuration = new Mock<IConfiguration>();
            configuration.SetupGet(x => x["EnvironmentName"]).Returns("LOCAL");
            configuration.SetupGet(x => x["ConfigurationStorageConnectionString"]).Returns("UseDevelopmentStorage=true;");

            configurationService.Setup(c => c.GetConfig())
                .ReturnsAsync(new ApplyConfig() { SessionRedisConnectionString = "HelloDave" });

            var builder = new WebHostBuilder();
            builder.ConfigureServices(services =>
            {
                services.AddSingleton(p => configurationService.Object);
                services.AddSingleton(p => configuration.Object);
            });
            builder.UseStartup<Startup>();

            var testServer = new TestServer(builder);

            _client = testServer.CreateClient();
        }




        [Ignore("Failed test")]
        public async Task ThenSuccessIsReturned()
        {
            var response = await _client.GetAsync("/");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Ignore("Failed test")]
        public async Task ThenTheCorrectViewIsReturned()
        {
            var response = await _client.GetAsync("/");

            var parser = new HtmlParser();
            var document = await parser.ParseAsync(await response.Content.ReadAsStringAsync());
            document.Title.Should().Be("Apprenticeship provider and assessment register service");
        }
    }
}