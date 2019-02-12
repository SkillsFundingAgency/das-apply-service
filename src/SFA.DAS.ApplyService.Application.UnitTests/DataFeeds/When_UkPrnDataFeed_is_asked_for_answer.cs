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
    public class When_UkPrnDataFeed_is_asked_for_answer
    {
        private Mock<IOrganisationRepository> _organisationRepository;
        private UkPrnDataFeed _ukPrnDataFeed;
        private Guid _applicationId;

        [SetUp]
        public void SetUp()
        {
            _applicationId = Guid.NewGuid();
            
            _organisationRepository = new Mock<IOrganisationRepository>();
            _organisationRepository.Setup(r => r.GetOrganisationByApplicationId(_applicationId))
                .ReturnsAsync(new Organisation {OrganisationUkprn = 9777});
            _ukPrnDataFeed = new UkPrnDataFeed(_organisationRepository.Object);
        }
        
        [Test]
        public void Then_repository_is_asked_for_the_organisation()
        {
            _ukPrnDataFeed.GetAnswer(_applicationId).Wait();
            
            _organisationRepository.Verify(r => r.GetOrganisationByApplicationId(_applicationId));
        }

        [Test]
        public void Then_feed_returns_correct_answer()
        {
            var answer = _ukPrnDataFeed.GetAnswer(_applicationId).Result;

            answer.Answer.Should().Be("9777");
        }
        
        [Test]
        public void And_ukprn_value_is_null_Then_returns_null()
        {
            _organisationRepository.Setup(r => r.GetOrganisationByApplicationId(_applicationId))
                .ReturnsAsync(new Organisation(){OrganisationUkprn = null});
            
            var answer = _ukPrnDataFeed.GetAnswer(_applicationId).Result;

            answer.Should().BeNull();
        }   

        [Test]
        public void Then_invalid_applicationId_returns_bad_request_exception()
        {
            _organisationRepository.Setup(r => r.GetOrganisationByApplicationId(_applicationId))
                .ReturnsAsync(default(Organisation));
            
            _ukPrnDataFeed.Invoking(df => df.GetAnswer(_applicationId).Wait()).Should().Throw<ArgumentException>();
        }
    }
}