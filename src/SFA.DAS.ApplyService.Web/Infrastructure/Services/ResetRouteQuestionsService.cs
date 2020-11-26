using System;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.Domain.Roatp;
using SFA.DAS.ApplyService.Web.Infrastructure.Interfaces;

namespace SFA.DAS.ApplyService.Web.Infrastructure.Services
{
    public class ResetRouteQuestionsService: IResetRouteQuestionsService
    {
        private readonly IQnaApiClient _qnaApiClient;

        public ResetRouteQuestionsService(IQnaApiClient qnaApiClient)
        {
            _qnaApiClient = qnaApiClient;
        }

        /// <summary>
        /// Resets the all pages when it would have been dependant (aka branched) on a particular route.
        /// </summary>
        /// <param name="applicationId">The application to reset</param>
        /// <param name="routeId">The new route</param>
        public async Task ResetRouteQuestions(Guid applicationId, int routeId)
        {
            await ResetDescribeYourOrganisationSectionTags(applicationId, routeId);
            await ResetTypeOfApprenticeshipTrainingSectionTags(applicationId, routeId);
        }

        private async Task ResetDescribeYourOrganisationSectionTags(Guid applicationId, int routeId)
        {
            const int sequenceNo = RoatpWorkflowSequenceIds.YourOrganisation;
            const int sectionNo = RoatpWorkflowSectionIds.YourOrganisation.DescribeYourOrganisation;

            switch (routeId)
            {
                case ApplicationRoute.MainProviderApplicationRoute:
                case ApplicationRoute.SupportingProviderApplicationRoute:
                    await _qnaApiClient.ResetPageAnswersBySequenceAndSectionNumber(applicationId, sequenceNo, sectionNo, RoatpWorkflowPageIds.DescribeYourOrganisation.EmployerStartPage);
                    break;
                case ApplicationRoute.EmployerProviderApplicationRoute:
                    await _qnaApiClient.ResetPageAnswersBySequenceAndSectionNumber(applicationId, sequenceNo, sectionNo, RoatpWorkflowPageIds.DescribeYourOrganisation.MainSupportingStartPage);
                    break;
            }
        }

        private async Task ResetTypeOfApprenticeshipTrainingSectionTags(Guid applicationId, int routeId)
        {
            const int sequenceNo = RoatpWorkflowSequenceIds.PlanningApprenticeshipTraining;
            const int sectionNo = RoatpWorkflowSectionIds.PlanningApprenticeshipTraining.TypeOfApprenticeshipTraining;

            switch (routeId)
            {
                case ApplicationRoute.MainProviderApplicationRoute:
                    await _qnaApiClient.ResetPageAnswersBySequenceAndSectionNumber(applicationId, sequenceNo, sectionNo, RoatpWorkflowPageIds.PlanningApprenticeshipTraining.TypeOfApprenticeshipTraining_Employer);
                    await _qnaApiClient.ResetPageAnswersBySequenceAndSectionNumber(applicationId, sequenceNo, sectionNo, RoatpWorkflowPageIds.PlanningApprenticeshipTraining.TypeOfApprenticeshipTraining_Supporting);
                    await _qnaApiClient.ResetPageAnswersBySequenceAndSectionNumber(applicationId, sequenceNo, sectionNo, RoatpWorkflowPageIds.PlanningApprenticeshipTraining.ApplicationFrameworks_Supporting);
                    await _qnaApiClient.ResetPageAnswersBySequenceAndSectionNumber(applicationId, sequenceNo, sectionNo, RoatpWorkflowPageIds.PlanningApprenticeshipTraining.OrganisationTransition_Supporting);
                    await _qnaApiClient.ResetPageAnswersBySequenceAndSectionNumber(applicationId, sequenceNo, sectionNo, RoatpWorkflowPageIds.PlanningApprenticeshipTraining.OnlyDeliveringApprenticeshipFrameworks_Supporting);
                    break;
                case ApplicationRoute.SupportingProviderApplicationRoute:
                    await _qnaApiClient.ResetPageAnswersBySequenceAndSectionNumber(applicationId, sequenceNo, sectionNo, RoatpWorkflowPageIds.PlanningApprenticeshipTraining.TypeOfApprenticeshipTraining_Main);
                    await _qnaApiClient.ResetPageAnswersBySequenceAndSectionNumber(applicationId, sequenceNo, sectionNo, RoatpWorkflowPageIds.PlanningApprenticeshipTraining.TypeOfApprenticeshipTraining_Employer);
                    await _qnaApiClient.ResetPageAnswersBySequenceAndSectionNumber(applicationId, sequenceNo, sectionNo, RoatpWorkflowPageIds.PlanningApprenticeshipTraining.ApplicationFrameworks_MainEmployer);
                    await _qnaApiClient.ResetPageAnswersBySequenceAndSectionNumber(applicationId, sequenceNo, sectionNo, RoatpWorkflowPageIds.PlanningApprenticeshipTraining.OrganisationTransition_MainEmployer);
                    await _qnaApiClient.ResetPageAnswersBySequenceAndSectionNumber(applicationId, sequenceNo, sectionNo, RoatpWorkflowPageIds.PlanningApprenticeshipTraining.OnlyDeliveringApprenticeshipFrameworks_MainEmployer);
                    break;
                case ApplicationRoute.EmployerProviderApplicationRoute:
                    await _qnaApiClient.ResetPageAnswersBySequenceAndSectionNumber(applicationId, sequenceNo, sectionNo, RoatpWorkflowPageIds.PlanningApprenticeshipTraining.TypeOfApprenticeshipTraining_Main);
                    await _qnaApiClient.ResetPageAnswersBySequenceAndSectionNumber(applicationId, sequenceNo, sectionNo, RoatpWorkflowPageIds.PlanningApprenticeshipTraining.TypeOfApprenticeshipTraining_Supporting);
                    await _qnaApiClient.ResetPageAnswersBySequenceAndSectionNumber(applicationId, sequenceNo, sectionNo, RoatpWorkflowPageIds.PlanningApprenticeshipTraining.ApplicationFrameworks_Supporting);
                    await _qnaApiClient.ResetPageAnswersBySequenceAndSectionNumber(applicationId, sequenceNo, sectionNo, RoatpWorkflowPageIds.PlanningApprenticeshipTraining.OrganisationTransition_Supporting);
                    await _qnaApiClient.ResetPageAnswersBySequenceAndSectionNumber(applicationId, sequenceNo, sectionNo, RoatpWorkflowPageIds.PlanningApprenticeshipTraining.OnlyDeliveringApprenticeshipFrameworks_Supporting);
                    break;
            }
        }
    }
}
