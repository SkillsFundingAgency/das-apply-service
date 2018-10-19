using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application;
using SFA.DAS.ApplyService.Application.Users;
using SFA.DAS.ApplyService.Configuration;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.InternalApi.IntegrationTests
{
    [TestFixture]
    public class WhenCallingInviteUser
    {
        private HttpClient _client;

        [SetUp]
        public void Setup()
        {
            var configurationService = new Mock<IConfigurationService>();
            var contactRepository = new Mock<IContactRepository>();
            contactRepository.Setup(r => r.CreateContact("email@email.com", "Fred", "Jones", "DfESignIn"))
                .ReturnsAsync(new Contact() {Id = Guid.NewGuid()});
                
            var dfeSignInService = new Mock<IDfeSignInService>();
            dfeSignInService.Setup(s => s.InviteUser("email@email.com", "Fred", "Jones", It.IsAny<Guid>()))
                .ReturnsAsync(new InviteUserResponse() {IsSuccess = true});
            var emailService = new Mock<IEmailService>();
            
            var factory = new WebApplicationFactory<Startup>();

            _client = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddSingleton(p => configurationService.Object);
                    services.AddTransient(p => contactRepository.Object);
                    services.AddTransient(p => dfeSignInService.Object);
                    services.AddTransient(p => emailService.Object);
                });
            }).CreateClient(new WebApplicationFactoryClientOptions() {AllowAutoRedirect = false});
        }
        
        [Test]
        public async Task ThenOkIsReturned()
        {
            var response = await _client.PostAsJsonAsync("/Account/", 
                new {Email = "email@email.com", GivenName = "Fred", FamilyName = "Jones"});
            
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}