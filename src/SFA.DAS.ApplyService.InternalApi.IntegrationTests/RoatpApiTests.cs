namespace SFA.DAS.ApplyService.InternalApi.IntegrationTests
{
    using System;
    using System.Linq;
    using System.Net.Http;
    using Configuration;
    using FluentAssertions;
    using Infrastructure;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Logging;
    using Models.Roatp;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class RoatpApiTests
    {
        private const string RoatpApiBaseAddress = "http://localhost:37951";

        private IConfigurationService _config;
        private const string ServiceName = "SFA.DAS.ApplyService";
        private const string Version = "1.0";
        private const string ConnectionString = "UseDevelopmentStorage=true;";

        private RoatpApiClient _apiClient;

        [SetUp]
        public void Before_each_test()
        {
            var hostingEnvironment = new Mock<IHostingEnvironment>();
            _config = new ConfigurationService(hostingEnvironment.Object, "LOCAL", ConnectionString, Version,
                ServiceName);

            _config.GetConfig().GetAwaiter().GetResult().RoatpApiAuthentication.ApiBaseAddress = RoatpApiBaseAddress;

            var logger = new Mock<ILogger<RoatpApiClient>>();

            _apiClient = new RoatpApiClient(new HttpClient(), logger.Object, _config, new RoatpTokenService(_config, hostingEnvironment.Object));
        }

        [Test]
        public void Client_retrieves_list_of_provider_types()
        {
            var providerTypes = _apiClient.GetProviderTypes().GetAwaiter().GetResult();

            providerTypes.Count().Should().Be(3);
        }

        [Test]
        public void Client_returns_reapply_status_for_existing_UKPRN_that_is_active()
        {
            var existingUKPRN = 10001123;
            
            var reapplyStatus = _apiClient.GetOrganisationRegisterStatus(existingUKPRN.ToString()).GetAwaiter().GetResult();

            reapplyStatus.ProviderTypeId.Should().Be(ProviderType.MainProvider);
            reapplyStatus.StatusId.Should().Be(OrganisationStatus.Active);
        }

        [Test]
        public void Client_returns_reapply_status_for_existing_UKPRN_that_was_removed()
        {
            var providerRequestedRemovalUKPRN = 10000066;

            var reapplyStatus = _apiClient.GetOrganisationRegisterStatus(providerRequestedRemovalUKPRN.ToString()).GetAwaiter().GetResult();

            reapplyStatus.ProviderTypeId.Should().Be(ProviderType.EmployerProvider);
            reapplyStatus.StatusId.Should().Be(OrganisationStatus.Removed);
        }

        [Test]
        public void Matching_UKPRN_returns_single_result()
        {
            var ukprn = "10001724";
            
            var result = _apiClient.GetUkrlpDetails(ukprn).GetAwaiter().GetResult();

            result.Should().NotBeNull();
            result.Results.Count.Should().Be(1);
            var matchResult = result.Results[0];
            matchResult.UKPRN.Should().Be(ukprn.ToString());
            matchResult.ProviderStatus.Should().Be("Active");
            matchResult.ContactDetails.FirstOrDefault(x => x.ContactType == "L").Should().NotBeNull();
            matchResult.VerificationDate.Should().NotBeNull();
            matchResult.VerificationDetails
                .FirstOrDefault(x => x.VerificationAuthority == "Companies House")
                .Should().NotBeNull();
            matchResult.ProviderAliases.Count.Should().Be(1);
        }

        [Test]
        public void Non_matching_UKPRN_returns_no_results()
        {
            var ukprn = "99998888";

            var result = _apiClient.GetUkrlpDetails(ukprn).GetAwaiter().GetResult();

            result.Should().NotBeNull();
            result.Results.Count.Should().Be(0);
        }

        [Test]
        public void Inactive_UKPRN_returns_no_results()
        {
            var ukprn = "10019227";

            var result = _apiClient.GetUkrlpDetails(ukprn).GetAwaiter().GetResult();

            result.Should().NotBeNull();
            result.Results.Count.Should().Be(0);
        }
    }
}
