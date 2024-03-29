﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using KellermanSoftware.CompareNetObjects;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply.Appeals.Commands;
using SFA.DAS.ApplyService.Application.Apply.Oversight;
using SFA.DAS.ApplyService.Application.Apply.Oversight.Queries.GetOversightDetails;
using SFA.DAS.ApplyService.Application.Apply.Oversight.Queries.GetOversightReview;
using SFA.DAS.ApplyService.Domain.QueryResults;
using SFA.DAS.ApplyService.InternalApi.Controllers;
using SFA.DAS.ApplyService.InternalApi.Services;
using SFA.DAS.ApplyService.InternalApi.Types.Requests.Oversight;
using SFA.DAS.ApplyService.InternalApi.Types.Responses.Oversight;
using SFA.DAS.ApplyService.Types;

namespace SFA.DAS.ApplyService.InternalApi.UnitTests
{
    [TestFixture]
    public class OversightControllerTests
    {
        private Mock<IMediator> _mediator;
        private Mock<IRegistrationDetailsService> _service;
        private OversightController _controller;
        private static readonly Fixture AutoFixture = new Fixture();

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
            var pendingOversights = new PendingOversightReviews 
            {
                Reviews = new List<PendingOversightReview> { 
                new PendingOversightReview
                {
                    ApplicationId = Guid.NewGuid(),
                    OrganisationName = "XXX Limited",
                    Ukprn = "12344321",
                    ProviderRoute = "Main",
                    ApplicationReferenceNumber = "APR000111"
                },
                new PendingOversightReview
                {
                    ApplicationId = Guid.NewGuid(),
                    OrganisationName = "ZZZ Limited",
                    Ukprn = "43211234",
                    ProviderRoute = "Employer",
                    ApplicationReferenceNumber = "APR000112",
                }
            }};

            _mediator
                .Setup(x => x.Send(It.IsAny<GetOversightsPendingRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(pendingOversights);

            var actualResult = await _controller.OversightsPending(null,null,null);
            Assert.AreEqual(pendingOversights.Reviews.Count, actualResult.Value.Reviews.Count);
        }

        [Test]
        public async Task Check_pending_results_are_as_expected()
        {
            var pendingOversights = new PendingOversightReviews
            {
                Reviews = new List<PendingOversightReview>
                {
                    new PendingOversightReview{
                        ApplicationId = Guid.NewGuid(),
                        OrganisationName = "XXX Limited",
                        Ukprn = "12344321",
                        ProviderRoute = "Main",
                        ApplicationReferenceNumber = "APR000111"
                    }
                }
            };

            _mediator
                .Setup(x => x.Send(It.IsAny<GetOversightsPendingRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(pendingOversights);

            var actualResult = await _controller.OversightsPending(null,null,null);
            var returnedOversight = actualResult.Value.Reviews.First();

            Assert.That(returnedOversight,Is.SameAs(pendingOversights.Reviews.First()));
        }

        [Test]
        public async Task Check_count_of_completed_results_are_correct()
        {
            var completedOversights = new CompletedOversightReviews
            {
                Reviews = new List<CompletedOversightReview> {
                new CompletedOversightReview
                {
                    ApplicationId = Guid.NewGuid(),
                    OrganisationName = "XXX Limited",
                    Ukprn = "12344321",
                    ProviderRoute = "Main",
                    ApplicationReferenceNumber = "APR000111"
                },
                new CompletedOversightReview
                {
                    ApplicationId = Guid.NewGuid(),
                    OrganisationName = "ZZZ Limited",
                    Ukprn = "43211234",
                    ProviderRoute = "Employer",
                    ApplicationReferenceNumber = "APR000112",
                }
            }
            };

            _mediator
                .Setup(x => x.Send(It.IsAny<GetOversightsCompletedRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(completedOversights);

            var actualResult = await _controller.OversightsCompleted(null, null, null);
            Assert.AreEqual(completedOversights.Reviews.Count, actualResult.Value.Reviews.Count);
        }

        [Test]
        public async Task Check_completed_results_are_as_expected()
        {
            var completedOversights = new CompletedOversightReviews
            {
                Reviews = new List<CompletedOversightReview>
                {
                    new CompletedOversightReview{
                        ApplicationId = Guid.NewGuid(),
                        OrganisationName = "XXX Limited",
                        Ukprn = "12344321",
                        ProviderRoute = "Main",
                        ApplicationReferenceNumber = "APR000111"
                    }
                }
            };

            _mediator
                .Setup(x => x.Send(It.IsAny<GetOversightsCompletedRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(completedOversights);

            var actualResult = await _controller.OversightsCompleted(null, null, null);
            var returnedOversight = actualResult.Value.Reviews.First();

            Assert.That(returnedOversight, Is.SameAs(completedOversights.Reviews.First()));
        }

        [Test]
        public async Task Check_count_of_appeals_pending_results_are_correct()
        {
            var pendingAppeals = new PendingAppealOutcomes
            {
                Reviews = new List<PendingAppealOutcome> {
                new PendingAppealOutcome
                {
                    ApplicationId = Guid.NewGuid(),
                    OrganisationName = "XXX Limited",
                    Ukprn = "12344321",
                    ProviderRoute = "Main",
                    ApplicationReferenceNumber = "APR000111"
                },
                new PendingAppealOutcome
                {
                    ApplicationId = Guid.NewGuid(),
                    OrganisationName = "ZZZ Limited",
                    Ukprn = "43211234",
                    ProviderRoute = "Employer",
                    ApplicationReferenceNumber = "APR000112",
                }
            }
            };

            _mediator
                .Setup(x => x.Send(It.IsAny<GetPendingAppealOutcomesRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(pendingAppeals);

            var actualResult = await _controller.AppealsPending(null, null, null);
            Assert.AreEqual(pendingAppeals.Reviews.Count, actualResult.Value.Reviews.Count);
        }

        [Test]
        public async Task Check_appeals_pending_results_are_as_expected()
        {
            var pendingAppeals = new PendingAppealOutcomes
            {
                Reviews = new List<PendingAppealOutcome>
                {
                    new PendingAppealOutcome{
                        ApplicationId = Guid.NewGuid(),
                        OrganisationName = "XXX Limited",
                        Ukprn = "12344321",
                        ProviderRoute = "Main",
                        ApplicationReferenceNumber = "APR000111"
                    }
                }
            };

            _mediator
                .Setup(x => x.Send(It.IsAny<GetPendingAppealOutcomesRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(pendingAppeals);

            var actualResult = await _controller.AppealsPending(null, null, null);
            var returnedAppeal = actualResult.Value.Reviews.First();

            Assert.That(returnedAppeal, Is.SameAs(pendingAppeals.Reviews.First()));
        }

        [Test]
        public async Task Check_count_of_appeals_completed_results_are_correct()
        {
            var completedAppeals = new CompletedAppealOutcomes
            {
                Reviews = new List<CompletedAppealOutcome> {
                new CompletedAppealOutcome
                {
                    ApplicationId = Guid.NewGuid(),
                    OrganisationName = "XXX Limited",
                    Ukprn = "12344321",
                    ProviderRoute = "Main",
                    ApplicationReferenceNumber = "APR000111"
                },
                new CompletedAppealOutcome
                {
                    ApplicationId = Guid.NewGuid(),
                    OrganisationName = "ZZZ Limited",
                    Ukprn = "43211234",
                    ProviderRoute = "Employer",
                    ApplicationReferenceNumber = "APR000112",
                }
            }
            };

            _mediator
                .Setup(x => x.Send(It.IsAny<GetCompletedAppealOutcomesRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(completedAppeals);

            var actualResult = await _controller.AppealsCompleted(null, null, null);
            Assert.AreEqual(completedAppeals.Reviews.Count, actualResult.Value.Reviews.Count);
        }

        [Test]
        public async Task Check_appeals_completed_results_are_as_expected()
        {
            var completedAppeals = new CompletedAppealOutcomes
            {
                Reviews = new List<CompletedAppealOutcome>
                {
                    new CompletedAppealOutcome{
                        ApplicationId = Guid.NewGuid(),
                        OrganisationName = "XXX Limited",
                        Ukprn = "12344321",
                        ProviderRoute = "Main",
                        ApplicationReferenceNumber = "APR000111"
                    }
                }
            };

            _mediator
                .Setup(x => x.Send(It.IsAny<GetCompletedAppealOutcomesRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(completedAppeals);

            var actualResult = await _controller.AppealsCompleted(null, null, null);
            var returnedAppeal = actualResult.Value.Reviews.First();

            Assert.That(returnedAppeal, Is.SameAs(completedAppeals.Reviews.First()));
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
                OversightStatus = OversightReviewStatus.InProgress
            };

            _mediator
                .Setup(x => x.Send(It.IsAny<GetOversightApplicationDetailsRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(oversight);

            var actualResult = await _controller.OversightDetails(applicationId);

            var returnedOversight = actualResult.Value;

            Assert.That(returnedOversight, Is.SameAs(oversight));
        }

        [TestCase(OversightReviewStatus.Successful)]
        [TestCase(OversightReviewStatus.Unsuccessful)]
        public async Task Record_oversight_outcome_updates_oversight_status_and_determined_date(OversightReviewStatus oversightStatus)
        {
            var command = new RecordOversightOutcomeCommand
            {
                OversightStatus = oversightStatus,
                ApplicationId = Guid.NewGuid(),
                UserId = "User Id",
                UserName = "Test user"
            };

            _mediator.Setup(x => x.Send(command, It.IsAny<CancellationToken>())).ReturnsAsync(true);

            var result = await _controller.RecordOversightOutcome(command);
            result.Should().NotBeNull();
            result.Value.Should().BeTrue();
        }

        [Test]
        public async Task Record_oversight_gateway_fail_outcome_updates_oversight_status_and_determined_date()
        {
            var command = new RecordOversightGatewayFailOutcomeCommand
            {
                ApplicationId = Guid.NewGuid(),
                UserId = "User Id",
                UserName = "Test user"
            };

            _mediator.Setup(x => x.Send(command, It.IsAny<CancellationToken>())).ReturnsAsync(Unit.Value);

            var result = await _controller.RecordOversightGatewayFailOutcome(command);
            result.Should().BeOfType<OkResult>();
        }


        [Test]
        public async Task Record_oversight_gateway_removed_outcome_updates_oversight_status_and_determined_date()
        {
            var command = new RecordOversightGatewayRemovedOutcomeCommand
            {
                ApplicationId = Guid.NewGuid(),
                UserId = "User Id",
                UserName = "Test user"
            };

            _mediator.Setup(x => x.Send(command, It.IsAny<CancellationToken>())).ReturnsAsync(Unit.Value);

            var result = await _controller.RecordOversightGatewayRemovedOutcome(command);
            result.Should().BeOfType<OkResult>();
        }

        [Test]
        public async Task Get_registration_details_retrieves_information_for_adding_provider_to_register()
        {
            var applicationId = Guid.NewGuid();
            var response = new RoatpRegistrationDetails
            {
                UKPRN = "10001234",
                LegalName = "Legal Name",
                TradingName = "Trading Name",
                ProviderTypeId = 1,
                OrganisationTypeId = 2,
                CompanyNumber = "5550003",
                CharityNumber = "125622"
            };

            _service.Setup(x => x.GetRegistrationDetails(applicationId)).ReturnsAsync(response);

            var result = await _controller.GetRegistrationDetails(applicationId);

            result.Value.UKPRN.Should().Be(response.UKPRN);
        }

        [Test]
        public async Task OversightReview_Gets_OversightReview_For_Application()
        {
            var request = new GetOversightReviewRequest();
            var queryResult = AutoFixture.Create<GetOversightReviewQueryResult>();

            _mediator.Setup(x => x.Send(It.IsAny<GetOversightReviewQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(queryResult);

            var result = await _controller.OversightReview(request);
            result.Should().BeOfType<ActionResult<GetOversightReviewResponse>>();

            var compareLogic = new CompareLogic(new ComparisonConfig { IgnoreObjectTypes = true });
            var comparisonResult = compareLogic.Compare(queryResult, result);
            Assert.IsTrue(comparisonResult.AreEqual);
        }
    }
}
