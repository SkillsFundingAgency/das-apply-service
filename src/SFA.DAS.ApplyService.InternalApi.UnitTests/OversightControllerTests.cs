using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply.Oversight;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.InternalApi.Controllers;
using SFA.DAS.ApplyService.InternalApi.Services;

namespace SFA.DAS.ApplyService.InternalApi.UnitTests
{
    [TestFixture]
    public class OversightControllerTests
    {
        private Mock<IMediator> _mediator;
        private Mock<IRegistrationDetailsService> _service;
        private OversightController _controller;


        [SetUp]
        public void Before_each_test()
        {
            _mediator = new Mock<IMediator>();
            _service = new Mock<IRegistrationDetailsService>();
            _controller = new OversightController(_mediator.Object, _service.Object);
        }

        [Test]
        public async Task Check_count_of_pending_results_are_correct()
        {
            var pendingOversights = new List<ApplicationOversightDetails>
            {
                new ApplicationOversightDetails
                {
                    Id = Guid.NewGuid(),
                    ApplicationId = Guid.NewGuid(),
                    OrganisationName = "XXX Limited",
                    Ukprn = "12344321",
                    ProviderRoute = "Main",
                    ApplicationReferenceNumber = "APR000111",
                    OversightStatus = "New"
                },
                new ApplicationOversightDetails
                {
                    Id = Guid.NewGuid(),
                    ApplicationId = Guid.NewGuid(),
                    OrganisationName = "ZZZ Limited",
                    Ukprn = "43211234",
                    ProviderRoute = "Employer",
                    ApplicationReferenceNumber = "APR000112",
                    OversightStatus = "New"
                }
            };

            _mediator
                .Setup(x => x.Send(It.IsAny<GetOversightsPendingRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(pendingOversights);

            var actualResult = await _controller.OversightsPending();

           Assert.AreEqual(pendingOversights.Count, actualResult.Value.Count);

        }

        [Test]
        public async Task Check_pending_results_are_as_expected()
        {
            var oversight = new ApplicationOversightDetails
            {
                Id = Guid.NewGuid(),
                ApplicationId = Guid.NewGuid(),
                OrganisationName = "XXX Limited",
                Ukprn = "12344321",
                ProviderRoute = "Main",
                ApplicationReferenceNumber = "APR000111",
                OversightStatus = "New"
            };

            var pendingOversights = new List<ApplicationOversightDetails>
            {
               oversight
            };

            _mediator
                .Setup(x => x.Send(It.IsAny<GetOversightsPendingRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(pendingOversights);

            var actualResult = await _controller.OversightsPending();

            var returnedOversight = actualResult.Value[0];

            Assert.AreEqual(oversight.Id, returnedOversight.Id);
            Assert.AreEqual(oversight.ApplicationId, returnedOversight.ApplicationId);
            Assert.AreEqual(oversight.OrganisationName, returnedOversight.OrganisationName);
            Assert.AreEqual(oversight.Ukprn, returnedOversight.Ukprn);
            Assert.AreEqual(oversight.ProviderRoute, returnedOversight.ProviderRoute);
            Assert.AreEqual(oversight.ApplicationReferenceNumber, returnedOversight.ApplicationReferenceNumber);
            Assert.AreEqual(oversight.OversightStatus, returnedOversight.OversightStatus);
        }

        [TestCase(OversightReviewStatus.Successful)]
        [TestCase(OversightReviewStatus.Unsuccessful)]
        public async Task Record_oversight_outcome_updates_oversight_status_and_determined_date(string oversightStatus)
        {
            var command = new RecordOversightOutcomeCommand
            {
                ApplicationDeterminedDate = DateTime.Now,
                OversightStatus = oversightStatus,
                ApplicationId = Guid.NewGuid(),
                UserName = "Test user"
            };

            _mediator.Setup(x => x.Send(command, It.IsAny<CancellationToken>())).ReturnsAsync(true);

            var result = await _controller.RecordOversightOutcome(command);
            result.Should().NotBeNull();
            result.Value.Should().BeTrue();
        }
    }
}
