using System;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.DataFeeds;
using SFA.DAS.ApplyService.Application.Organisations;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Application.UnitTests.DataFeeds
{
    [TestFixture]
    public class When_CharityNumberDataFeed_is_asked_for_answer
    {
        private Mock<IOrganisationRepository> _organisationRepository;
        private CharityNumberDataFeed _charityNumberDataFeed;
        private Guid _applicationId;

        [SetUp]
        public void SetUp()
        {
            _applicationId = Guid.NewGuid();
            
            _organisationRepository = new Mock<IOrganisationRepository>();
            _organisationRepository.Setup(r => r.GetOrganisationByApplicationId(_applicationId))
                .ReturnsAsync(new Organisation {OrganisationDetails = new OrganisationDetails(){CharityNumber = "CHAR9777"}});
            _charityNumberDataFeed = new CharityNumberDataFeed(_organisationRepository.Object);
        }
        
        [Test]
        public void Then_repository_is_asked_for_the_organisation()
        {
            _charityNumberDataFeed.GetAnswer(_applicationId).Wait();
            
            _organisationRepository.Verify(r => r.GetOrganisationByApplicationId(_applicationId));
        }

        [Test]
        public void Then_feed_returns_correct_answer()
        {
            var answer = _charityNumberDataFeed.GetAnswer(_applicationId).Result;

            answer.Answer.Should().Be("CHAR9777");
        }
        
        [Test]
        public void And_charityNumber_value_is_null_Then_returns_null()
        {
            _organisationRepository.Setup(r => r.GetOrganisationByApplicationId(_applicationId))
                .ReturnsAsync(new Organisation(){OrganisationUkprn = null});
            
            var answer = _charityNumberDataFeed.GetAnswer(_applicationId).Result;

            answer.Answer.Should().BeNull();
        }   

        [Test]
        public void Then_invalid_applicationId_returns_bad_request_exception()
        {
            _organisationRepository.Setup(r => r.GetOrganisationByApplicationId(_applicationId))
                .ReturnsAsync(default(Organisation));
            
            _charityNumberDataFeed.Invoking(df => df.GetAnswer(_applicationId).Wait()).Should().Throw<ArgumentException>();
        }
    }
}