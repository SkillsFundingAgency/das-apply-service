using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using KellermanSoftware.CompareNetObjects;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Services.Assessor;
using SFA.DAS.ApplyService.Domain.Apply.Clarification;
using SFA.DAS.ApplyService.InternalApi.Types.Assessor;
using SFA.DAS.ApplyService.Web.Configuration;
using SFA.DAS.ApplyService.Web.Infrastructure;
using SFA.DAS.ApplyService.Web.Services;
using SFA.DAS.ApplyService.Web.ViewModels.Roatp;

namespace SFA.DAS.ApplyService.Web.UnitTests.Services
{
    [TestFixture]
    public class OverallOutcomeAugmentationServiceTests
    {
        private Mock<IApplicationApiClient> _apiClient;
        private Mock<IQnaApiClient> _qnaApiClient;
        private Mock<IAssessorLookupService> _assessorLookupService;
        private Guid _applicationId;
        private ApplicationSummaryWithModeratorDetailsViewModel _model;
        private string _userId;
        private OverallOutcomeAugmentationService _service;

        [SetUp]
        public void Before_each_test()
        {
            _applicationId = Guid.NewGuid();
            _qnaApiClient = new Mock<IQnaApiClient>();
            _apiClient = new Mock<IApplicationApiClient>();
            _assessorLookupService = new Mock<IAssessorLookupService>();
            _userId = "test";

            var sequences = new List<AssessorSequence>();

            var clarificationOutcomes = new List<ClarificationPageReviewOutcome>();

            _apiClient.Setup(x => x.GetClarificationSequences(_applicationId)).ReturnsAsync(sequences);
            _apiClient.Setup(x => x.GetAllClarificationPageReviewOutcomes(_applicationId, _userId)).ReturnsAsync(clarificationOutcomes);

            _service = new OverallOutcomeAugmentationService(_apiClient.Object, _qnaApiClient.Object,
                _assessorLookupService.Object);


        }


        [Test]
        public async Task no_failed_moderator_question_returns_model_unchanged()
        {
            var modelPreUpdate = GetCopyOfModel();
            var model = GetCopyOfModel();

            await _service.AugmentModelWithModerationFailDetails(model, _userId);

            modelPreUpdate.Should().BeEquivalentTo(model);
        }



        private ApplicationSummaryWithModeratorDetailsViewModel GetCopyOfModel()
        {
            var model = new ApplicationSummaryWithModeratorDetailsViewModel
            { ApplicationId = _applicationId };

            return model;
        }
    }
}
