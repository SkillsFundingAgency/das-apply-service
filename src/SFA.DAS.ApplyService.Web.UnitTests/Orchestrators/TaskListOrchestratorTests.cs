using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Domain.Roatp;
using SFA.DAS.ApplyService.Web.Infrastructure;
using SFA.DAS.ApplyService.Web.Orchestrators;
using SFA.DAS.ApplyService.Web.Services;

namespace SFA.DAS.ApplyService.Web.UnitTests.Orchestrators
{
    [TestFixture]
    public class TaskListOrchestratorTests
    {
        private TaskListOrchestrator _orchestrator;

        private readonly Guid _applicationId = Guid.NewGuid();
        private readonly Guid _userId = Guid.NewGuid();

        private Mock<IApplicationApiClient> _applicationApiClient;
        private Mock<IQnaApiClient> _qnaApiClient;
        private Mock<IRoatpOrganisationVerificationService> _roatpOrganisationVerificationService;
        private Mock<IRoatpTaskListWorkflowService> _roatpTaskListWorkflowService;
        private Mock<INotRequiredOverridesService> _notRequiredOverridesService;

        private List<ApplicationSequence> _sequences;

        [SetUp]
        public void Arrange()
        {
            _applicationApiClient = new Mock<IApplicationApiClient>();
            _qnaApiClient = new Mock<IQnaApiClient>();
            _roatpOrganisationVerificationService = new Mock<IRoatpOrganisationVerificationService>();
            _roatpTaskListWorkflowService = new Mock<IRoatpTaskListWorkflowService>();
            _notRequiredOverridesService = new Mock<INotRequiredOverridesService>();


            _applicationApiClient.Setup(x => x.GetOrganisationByUserId(_userId)).ReturnsAsync(() => new Organisation());

            _qnaApiClient.Setup(x => x.GetAnswerByTag(_applicationId, "ApplyProviderRoute", null))
                .ReturnsAsync(new Answer());

            _roatpOrganisationVerificationService.Setup(x => x.GetOrganisationVerificationStatus(_applicationId))
                .ReturnsAsync(() => new OrganisationVerificationStatus());

            _notRequiredOverridesService.Setup(x => x.RefreshNotRequiredOverridesAsync(_applicationId)).Returns(()=> Task.CompletedTask);

            _sequences = GenerateTestSequences();

            _roatpTaskListWorkflowService.Setup(x => x.GetApplicationSequencesAsync(_applicationId)).ReturnsAsync(()=> _sequences.ToArray());

            _roatpTaskListWorkflowService
                .Setup(x => x.SectionStatus(_applicationId, It.IsAny<int>(), It.IsAny<int>(), It.IsAny<List<ApplicationSequence>>(), It.IsAny<OrganisationVerificationStatus>()))
                .Returns(() => "Completed");

            _roatpTaskListWorkflowService
                .Setup(x => x.PreviousSectionCompleted(_applicationId, It.IsAny<int>(), It.IsAny<int>(),
                    It.IsAny<List<ApplicationSequence>>(), It.IsAny<OrganisationVerificationStatus>()))
                .Returns(() => true);

            _orchestrator = new TaskListOrchestrator(_applicationApiClient.Object,
                _qnaApiClient.Object,
                _roatpOrganisationVerificationService.Object,
                _roatpTaskListWorkflowService.Object,
                _notRequiredOverridesService.Object);
        }

        private List<ApplicationSequence> GenerateTestSequences()
        {
            var result = new List<ApplicationSequence>();

            for (var i = 1; i < 10; i++)
            {
                var newSequence = new ApplicationSequence { SequenceId = i, Sections = new List<ApplicationSection>() };

                for (var j = 1; j < 6; j++)
                {
                    newSequence.Sections.Add(new ApplicationSection{SectionId = j, SequenceId = i});
                }

                result.Add(newSequence);
            }

            return result;
        }

        [Test]
        public async Task GetTaskListViewModel_refreshes_not_required_overrides()
        {
            await _orchestrator.GetTaskListViewModel(_applicationId, _userId);

            _notRequiredOverridesService.Verify(x => x.RefreshNotRequiredOverridesAsync(_applicationId), Times.Once);
        }

        [Test]
        public async Task GetTaskListViewModel_result_contains_all_sequences_and_sections()
        {
            var result = await _orchestrator.GetTaskListViewModel(_applicationId, _userId);

            Assert.AreEqual(_sequences.Count, result.Sequences.Count);

            foreach (var sequence in _sequences)
            {
                var resultSequence = result.Sequences.First(x => x.Id == sequence.SequenceId);
                Assert.AreEqual(sequence.Sections.Count, resultSequence.Sections.Count);
            }
        }

        [Test]
        public async Task GetTaskListViewModel_other_sequences_are_locked_if_sequence_1_is_not_complete()
        {
            _roatpTaskListWorkflowService
                .Setup(x => x.SectionStatus(_applicationId, 1, It.IsAny<int>(), It.IsAny<List<ApplicationSequence>>(), It.IsAny<OrganisationVerificationStatus>()))
                .Returns(() => "");

            var result = await _orchestrator.GetTaskListViewModel(_applicationId, _userId);

            foreach (var sequence in _sequences.Where(x => x.SequenceId > 1))
            {
                var resultSequence = result.Sequences.First(x => x.Id == sequence.SequenceId);
                Assert.IsTrue(resultSequence.Sections.All(x => x.IsLocked));
            }
        }
    }
}
