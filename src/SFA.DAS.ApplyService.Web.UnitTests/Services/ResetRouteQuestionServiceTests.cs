using System;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.Domain.Roatp;
using SFA.DAS.ApplyService.Web.Infrastructure;
using SFA.DAS.ApplyService.Web.Infrastructure.Services;

namespace SFA.DAS.ApplyService.Web.UnitTests.Services
{
    [TestFixture]
    public class ResetRouteQuestionServiceTests
    {
        private ResetRouteQuestionsService _service;
        private Mock<IQnaApiClient> _qnaApiClient;

        private Guid _applicationId;

        [SetUp]
        public void Before_each_test()
        {
            _applicationId = Guid.NewGuid();
            _qnaApiClient = new Mock<IQnaApiClient>();
            _service = new ResetRouteQuestionsService(_qnaApiClient.Object);
        }

        [Test]
        public async Task Check_reset_for_main_route_resets_employer_and_supporting_questions()
        {
            var routeId = ApplicationRoute.MainProviderApplicationRoute;
            await _service.ResetRouteQuestions(_applicationId, routeId);

            _qnaApiClient.Verify(x => x.ResetPageAnswersBySequenceAndSectionNumber(_applicationId, 1, 4, RoatpWorkflowPageIds.DescribeYourOrganisation.EmployerStartPage));
            _qnaApiClient.Verify(x => x.ResetPageAnswersBySequenceAndSectionNumber(_applicationId, 1, 4, RoatpWorkflowPageIds.DescribeYourOrganisation.MainSupportingStartPage), Times.Never);

            _qnaApiClient.Verify(x => x.ResetPageAnswersBySequenceAndSectionNumber(_applicationId, 6, 2, RoatpWorkflowPageIds.PlanningApprenticeshipTraining.TypeOfApprenticeshipTraining_Employer));
            _qnaApiClient.Verify(x => x.ResetPageAnswersBySequenceAndSectionNumber(_applicationId, 6, 2, RoatpWorkflowPageIds.PlanningApprenticeshipTraining.TypeOfApprenticeshipTraining_Supporting));
            _qnaApiClient.Verify(x => x.ResetPageAnswersBySequenceAndSectionNumber(_applicationId, 6, 2, RoatpWorkflowPageIds.PlanningApprenticeshipTraining.ApplicationFrameworks_Supporting));
            _qnaApiClient.Verify(x => x.ResetPageAnswersBySequenceAndSectionNumber(_applicationId, 6, 2, RoatpWorkflowPageIds.PlanningApprenticeshipTraining.OrganisationTransition_Supporting));
            _qnaApiClient.Verify(x => x.ResetPageAnswersBySequenceAndSectionNumber(_applicationId, 6, 2, RoatpWorkflowPageIds.PlanningApprenticeshipTraining.OnlyDeliveringApprenticeshipFrameworks_Supporting));
            _qnaApiClient.Verify(x => x.ResetPageAnswersBySequenceAndSectionNumber(_applicationId, 6, 2, RoatpWorkflowPageIds.PlanningApprenticeshipTraining.TypeOfApprenticeshipTraining_Main), Times.Never);
        }


        [Test]
        public async Task Check_reset_for_supporting_route_resets_employer_and_main_questions()
        {
            var routeId = ApplicationRoute.SupportingProviderApplicationRoute;
            await _service.ResetRouteQuestions(_applicationId, routeId);

            _qnaApiClient.Verify(x => x.ResetPageAnswersBySequenceAndSectionNumber(_applicationId, 1, 4, RoatpWorkflowPageIds.DescribeYourOrganisation.EmployerStartPage));
            _qnaApiClient.Verify(x => x.ResetPageAnswersBySequenceAndSectionNumber(_applicationId, 1, 4, RoatpWorkflowPageIds.DescribeYourOrganisation.MainSupportingStartPage), Times.Never);

            _qnaApiClient.Verify(x => x.ResetPageAnswersBySequenceAndSectionNumber(_applicationId, 6, 2, RoatpWorkflowPageIds.PlanningApprenticeshipTraining.TypeOfApprenticeshipTraining_Employer));
            _qnaApiClient.Verify(x => x.ResetPageAnswersBySequenceAndSectionNumber(_applicationId, 6, 2, RoatpWorkflowPageIds.PlanningApprenticeshipTraining.TypeOfApprenticeshipTraining_Main));
            _qnaApiClient.Verify(x => x.ResetPageAnswersBySequenceAndSectionNumber(_applicationId, 6, 2, RoatpWorkflowPageIds.PlanningApprenticeshipTraining.ApplicationFrameworks_MainEmployer));
            _qnaApiClient.Verify(x => x.ResetPageAnswersBySequenceAndSectionNumber(_applicationId, 6, 2, RoatpWorkflowPageIds.PlanningApprenticeshipTraining.OrganisationTransition_MainEmployer));
            _qnaApiClient.Verify(x => x.ResetPageAnswersBySequenceAndSectionNumber(_applicationId, 6, 2, RoatpWorkflowPageIds.PlanningApprenticeshipTraining.OnlyDeliveringApprenticeshipFrameworks_MainEmployer));
            _qnaApiClient.Verify(x => x.ResetPageAnswersBySequenceAndSectionNumber(_applicationId, 6, 2, RoatpWorkflowPageIds.PlanningApprenticeshipTraining.TypeOfApprenticeshipTraining_Supporting), Times.Never);
        }

        [Test]
        public async Task Check_reset_for_employer_route_resets_supporting_and_main_questions()
        {
            var routeId = ApplicationRoute.EmployerProviderApplicationRoute;
            await _service.ResetRouteQuestions(_applicationId, routeId);

            _qnaApiClient.Verify(x => x.ResetPageAnswersBySequenceAndSectionNumber(_applicationId, 1, 4, RoatpWorkflowPageIds.DescribeYourOrganisation.MainSupportingStartPage));
            _qnaApiClient.Verify(x => x.ResetPageAnswersBySequenceAndSectionNumber(_applicationId, 1, 4, RoatpWorkflowPageIds.DescribeYourOrganisation.EmployerStartPage), Times.Never);

            _qnaApiClient.Verify(x => x.ResetPageAnswersBySequenceAndSectionNumber(_applicationId, 6, 2, RoatpWorkflowPageIds.PlanningApprenticeshipTraining.TypeOfApprenticeshipTraining_Main));
            _qnaApiClient.Verify(x => x.ResetPageAnswersBySequenceAndSectionNumber(_applicationId, 6, 2, RoatpWorkflowPageIds.PlanningApprenticeshipTraining.TypeOfApprenticeshipTraining_Supporting));
            _qnaApiClient.Verify(x => x.ResetPageAnswersBySequenceAndSectionNumber(_applicationId, 6, 2, RoatpWorkflowPageIds.PlanningApprenticeshipTraining.ApplicationFrameworks_Supporting));
            _qnaApiClient.Verify(x => x.ResetPageAnswersBySequenceAndSectionNumber(_applicationId, 6, 2, RoatpWorkflowPageIds.PlanningApprenticeshipTraining.OrganisationTransition_Supporting));
            _qnaApiClient.Verify(x => x.ResetPageAnswersBySequenceAndSectionNumber(_applicationId, 6, 2, RoatpWorkflowPageIds.PlanningApprenticeshipTraining.OnlyDeliveringApprenticeshipFrameworks_Supporting));
            _qnaApiClient.Verify(x => x.ResetPageAnswersBySequenceAndSectionNumber(_applicationId, 6, 2, RoatpWorkflowPageIds.PlanningApprenticeshipTraining.TypeOfApprenticeshipTraining_Employer), Times.Never);
        }
    }
}
