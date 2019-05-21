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

            _apiClient = new RoatpApiClient(new HttpClient(), logger.Object, _config, new RoatpTokenService(_config));
        }

        [Test]
        public void Client_retrieves_list_of_provider_types()
        {
            var providerTypes = _apiClient.GetProviderTypes().GetAwaiter().GetResult();

            providerTypes.Count().Should().Be(3);
        }

        [Test]
        public void Client_returns_duplicate_check_result_of_true_for_existing_UKPRN()
        {
            var existingUKPRN = 10001123;

            var duplicateCheckResult = _apiClient.DuplicateUKPRNCheck(Guid.NewGuid(), existingUKPRN).GetAwaiter().GetResult();

            duplicateCheckResult.DuplicateFound.Should().BeTrue();
            duplicateCheckResult.DuplicateOrganisationId.Should().NotBe(Guid.Empty);
            duplicateCheckResult.DuplicateOrganisationName.Should().NotBeNullOrEmpty();
        }

        [Test]
        public void Client_returns_duplicate_check_results_of_false_for_new_UKPRN()
        {
            var existingUKPRN = 99998888;

            var duplicateCheckResult = _apiClient.DuplicateUKPRNCheck(Guid.NewGuid(), existingUKPRN).GetAwaiter().GetResult();

            duplicateCheckResult.DuplicateFound.Should().BeFalse();
            duplicateCheckResult.DuplicateOrganisationId.Should().Be(Guid.Empty);
            duplicateCheckResult.DuplicateOrganisationName.Should().BeNullOrEmpty();
        }

        [Test]
        public void Client_returns_reapply_status_for_existing_UKPRN_that_is_active()
        {
            var existingUKPRN = 10001123;

            var duplicateCheckResult = _apiClient.DuplicateUKPRNCheck(Guid.NewGuid(), existingUKPRN).GetAwaiter().GetResult();

            var reapplyStatus = _apiClient.GetOrganisationRegisterStatus(duplicateCheckResult.DuplicateOrganisationId).GetAwaiter().GetResult();

            reapplyStatus.ProviderTypeId.Should().Be(ProviderType.MainProvider);
            reapplyStatus.StatusId.Should().Be(OrganisationStatus.Active);
        }

        [Test]
        public void Client_returns_reapply_status_for_existing_UKPRN_that_was_removed()
        {
            var providerRequestedRemovalUKPRN = 10000066;

            var duplicateCheckResult = _apiClient.DuplicateUKPRNCheck(Guid.NewGuid(), providerRequestedRemovalUKPRN).GetAwaiter().GetResult();

            var reapplyStatus = _apiClient.GetOrganisationRegisterStatus(duplicateCheckResult.DuplicateOrganisationId).GetAwaiter().GetResult();

            reapplyStatus.ProviderTypeId.Should().Be(ProviderType.EmployerProvider);
            reapplyStatus.StatusId.Should().Be(OrganisationStatus.Removed);
        }
    }
}
