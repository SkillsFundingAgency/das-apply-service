using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ApplyService.Web.StartupExtensions;

namespace SFA.DAS.ApplyService.Web.UnitTests.StartupExtensions
{
    public class WhenGettingDomainExtensions
    {
        [TestCase("LocAL","")]
        [TestCase("TEST","test-apply.apprenticeships.education.gov.uk")]
        [TestCase("PRD","apply.apprenticeships.education.gov.uk")]
        [TestCase("prePROD","preprod-apply.apprenticeships.education.gov.uk")]
        public void Then_The_Domain_Is_Correct_For_Each_Environment(string environment, string expectedDomain)
        {
            var actual = DomainExtensions.GetDomain(environment);

            actual.Should().Be(expectedDomain);
        }
    
    }
}