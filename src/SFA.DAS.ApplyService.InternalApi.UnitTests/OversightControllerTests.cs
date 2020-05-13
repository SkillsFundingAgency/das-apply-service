﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply.Oversight;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.InternalApi.Controllers;

namespace SFA.DAS.ApplyService.InternalApi.UnitTests
{
    [TestFixture]
    public class OversightControllerTests
    {
        private Mock<IMediator> _mediator;
        private OversightController _controller;


        [SetUp]
        public void Before_each_test()
        {
            _mediator = new Mock<IMediator>();
            _controller = new OversightController(_mediator.Object);
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

            Assert.That(returnedOversight,Is.SameAs(oversight));
        }


        [Test]
        public async Task Check_oversight_details_result_is_as_expected()
        {
            var applicationId = Guid.NewGuid();

            var oversight = new ApplicationOversightDetails
            {
                Id = Guid.NewGuid(),
                ApplicationId = applicationId,
                OrganisationName = "XXX Limited",
                Ukprn = "12344321",
                ProviderRoute = "Main",
                ApplicationReferenceNumber = "APR000111",
                OversightStatus = "New"
            };

            _mediator
                .Setup(x => x.Send(It.IsAny<GetOversightDetailsRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(oversight);

            var actualResult = await _controller.OversightDetails(applicationId);

            var returnedOversight = actualResult.Value;

            Assert.That(returnedOversight, Is.SameAs(oversight));
        }
    }
}
