namespace SFA.DAS.ApplyService.Web.UnitTests
{
    using System;
    using FluentAssertions;
    using NUnit.Framework;
    using Validators;

    [TestFixture]
    public class IncorporationDateValidatorTests
    {
        [TestCase(1, 12)]
        [TestCase(2, 12)]
        [TestCase(3, 3)]
        public void Incorporation_date_is_less_than_threshold_for_application_route(int applicationRouteId, int months)
        {
            DateTime incorporationDate = DateTime.Today.AddMonths(-1 * months);
            incorporationDate = incorporationDate.AddDays(1);

            bool isValid = IncorporationDateValidator.IsValidIncorporationDate(applicationRouteId, incorporationDate);

            isValid.Should().BeFalse();
        }

        [TestCase(1, 12)]
        [TestCase(2, 12)]
        [TestCase(3, 3)]
        public void Incorporation_date_is_over_threshold_for_application_route(int applicationRouteId, int months)
        {
            DateTime incorporationDate = DateTime.Today.AddMonths(-1 * months);
            incorporationDate = incorporationDate.AddDays(-1);

            bool isValid = IncorporationDateValidator.IsValidIncorporationDate(applicationRouteId, incorporationDate);

            isValid.Should().BeTrue();
        }

        [TestCase(1, 12)]
        [TestCase(2, 12)]
        [TestCase(3, 3)]
        public void Incorporation_date_is_at_threshold_for_application_route(int applicationRouteId, int months)
        {
            DateTime incorporationDate = DateTime.Today.AddMonths(-1 * months);

            bool isValid = IncorporationDateValidator.IsValidIncorporationDate(applicationRouteId, incorporationDate);

            isValid.Should().BeTrue();
        }
    }
}
