namespace SFA.DAS.ApplyService.Web.UnitTests
{
    using System;
    using FluentAssertions;
    using NUnit.Framework;
    using Validators;

    [TestFixture]
    public class ProviderHistoryValidatorTests
    {
        [TestCase(1, 12)]
        [TestCase(2, 12)]
        [TestCase(3, 3)]
        public void Date_is_less_than_threshold_for_application_route(int applicationRouteId, int months)
        {
            DateTime compareDate = DateTime.Today.AddMonths(-1 * months);
            compareDate = compareDate.AddDays(1);

            bool isValid = ProviderHistoryValidator.HasSufficientHistory(applicationRouteId, compareDate);

            isValid.Should().BeFalse();
        }

        [TestCase(1, 12)]
        [TestCase(2, 12)]
        [TestCase(3, 3)]
        public void Date_is_over_threshold_for_application_route(int applicationRouteId, int months)
        {
            DateTime compareDate = DateTime.Today.AddMonths(-1 * months);
            compareDate = compareDate.AddDays(-1);

            bool isValid = ProviderHistoryValidator.HasSufficientHistory(applicationRouteId, compareDate);

            isValid.Should().BeTrue();
        }

        [TestCase(1, 12)]
        [TestCase(2, 12)]
        [TestCase(3, 3)]
        public void Date_is_at_threshold_for_application_route(int applicationRouteId, int months)
        {
            DateTime compareDate = DateTime.Today.AddMonths(-1 * months);

            bool isValid = ProviderHistoryValidator.HasSufficientHistory(applicationRouteId, compareDate);

            isValid.Should().BeTrue();
        }
    }
}
