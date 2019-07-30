
namespace SFA.DAS.ApplyService.Web.UnitTests
{
    using System;
    using Domain.Roatp;
    using FluentAssertions;
    using NUnit.Framework;
    using SFA.DAS.ApplyService.Web.Validators;

    [TestFixture]
    public class RoatpStatusValidatorTests
    {
        private RoatpStatusValidator _validator;

        [SetUp]
        public void Before_each_test()
        {
            _validator = new RoatpStatusValidator();
        }

        [Test]
        public void Provider_not_already_on_register_eligible_to_join()
        {
            var registerStatus = new OrganisationRegisterStatus {UkprnOnRegister = false};

            var eligible = _validator.ProviderEligibleToJoinRegister(registerStatus);

            eligible.Should().BeTrue();
        }

        [TestCase(OrganisationStatus.Active)]
        [TestCase(OrganisationStatus.ActiveNotTakingOnApprentices)]
        [TestCase(OrganisationStatus.Onboarding)]
        public void Provider_already_on_register_and_active_eligible_to_join(int organisationStatusId)
        {
            var registerStatus = new OrganisationRegisterStatus
            {
                UkprnOnRegister = true,
                OrganisationId = Guid.NewGuid(),
                StatusId = organisationStatusId,
                ProviderTypeId = ApplicationRoute.MainProviderApplicationRoute,
                RemovedReasonId = null,
                StatusDate = new DateTime(2018, 2, 3)
             };
            
            var eligible = _validator.ProviderEligibleToJoinRegister(registerStatus);

            eligible.Should().BeTrue();
        }

        [Test]
        public void Provider_removed_from_register_three_years_ago_eligible_to_join()
        {
            var removalDate = DateTime.Today.AddYears(-3);

            var registerStatus = new OrganisationRegisterStatus
            {
                UkprnOnRegister = true,
                OrganisationId = Guid.NewGuid(),
                StatusId = OrganisationStatus.Removed,
                ProviderTypeId = ApplicationRoute.MainProviderApplicationRoute,
                RemovedReasonId = RemovedReason.ChangeOfTradingStatus,
                StatusDate = removalDate
            };

            var eligible = _validator.ProviderEligibleToJoinRegister(registerStatus);

            eligible.Should().BeTrue();
        }

        [Test]
        public void Provider_removed_from_register_by_provider_request_less_than_three_years_ago_eligible_to_join()
        {
            var removalDate = DateTime.Today.AddYears(-3).AddDays(1);

            var registerStatus = new OrganisationRegisterStatus
            {
                UkprnOnRegister = true,
                OrganisationId = Guid.NewGuid(),
                StatusId = OrganisationStatus.Removed,
                ProviderTypeId = ApplicationRoute.MainProviderApplicationRoute,
                RemovedReasonId = RemovedReason.ProviderRequest,
                StatusDate = removalDate
            };

            var eligible = _validator.ProviderEligibleToJoinRegister(registerStatus);

            eligible.Should().BeTrue();
        }

        [TestCase(RemovedReason.Breach)]
        [TestCase(RemovedReason.ChangeOfTradingStatus)]
        [TestCase(RemovedReason.HighRiskPolicy)]
        [TestCase(RemovedReason.InadequateFinancialHealth)]
        [TestCase(RemovedReason.InadequateOfstedGrade)]
        [TestCase(RemovedReason.InternalError)]
        [TestCase(RemovedReason.Merger)]
        [TestCase(RemovedReason.MinimumStandardsNotMet)]
        [TestCase(RemovedReason.NonDirectDeliveryInTwelveMonthPeriod)]
        [TestCase(RemovedReason.Other)]
        [TestCase(RemovedReason.ProviderError)]
        public void Provider_removed_from_register_not_by_provider_request_less_than_three_years_ago_is_not_eligible_to_join(int removedReasonId)
        {
            var removalDate = DateTime.Today.AddYears(-3).AddDays(1);

            var registerStatus = new OrganisationRegisterStatus
            {
                UkprnOnRegister = true,
                OrganisationId = Guid.NewGuid(),
                StatusId = OrganisationStatus.Removed,
                ProviderTypeId = ApplicationRoute.MainProviderApplicationRoute,
                RemovedReasonId = removedReasonId,
                StatusDate = removalDate
            };

            var eligible = _validator.ProviderEligibleToJoinRegister(registerStatus);

            eligible.Should().BeFalse();
        }
    }
}
