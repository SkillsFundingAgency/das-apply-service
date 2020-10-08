using System;
using System.Linq;
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

        public async Task ResetRouteQuestions(Guid applicationId, int routeId)
        {

            switch (routeId)
            {
                case ApplicationRoute.MainProviderApplicationRoute:
                    await ResetSection14TasksForEmployerTags(applicationId);
                    await _qnaApiClient.ResetPageAnswersBySequenceAndSectionNumber(applicationId, 6, 2, RoatpWorkflowPageIds.PlanningApprenticeshipTraining.TypeOfApprenticeshipTraining_Employer);
                    await _qnaApiClient.ResetPageAnswersBySequenceAndSectionNumber(applicationId, 6, 2, RoatpWorkflowPageIds.PlanningApprenticeshipTraining.TypeOfApprenticeshipTraining_Supporting);
                    await _qnaApiClient.ResetPageAnswersBySequenceAndSectionNumber(applicationId, 6, 2, RoatpWorkflowPageIds.PlanningApprenticeshipTraining.ApplicationFrameworks_Supporting);
                    await _qnaApiClient.ResetPageAnswersBySequenceAndSectionNumber(applicationId, 6, 2, RoatpWorkflowPageIds.PlanningApprenticeshipTraining.OrganisationTransition_Supporting);
                    await _qnaApiClient.ResetPageAnswersBySequenceAndSectionNumber(applicationId, 6, 2, RoatpWorkflowPageIds.PlanningApprenticeshipTraining.OnlyDeliveringApprenticeshipFrameworks_Supporting);
                    break;
                case ApplicationRoute.SupportingProviderApplicationRoute:
                    await ResetSection14TasksForEmployerTags(applicationId);
                    await _qnaApiClient.ResetPageAnswersBySequenceAndSectionNumber(applicationId, 6, 2, RoatpWorkflowPageIds.PlanningApprenticeshipTraining.TypeOfApprenticeshipTraining_Main);
                    await _qnaApiClient.ResetPageAnswersBySequenceAndSectionNumber(applicationId, 6, 2, RoatpWorkflowPageIds.PlanningApprenticeshipTraining.TypeOfApprenticeshipTraining_Employer);
                    await _qnaApiClient.ResetPageAnswersBySequenceAndSectionNumber(applicationId, 6, 2, RoatpWorkflowPageIds.PlanningApprenticeshipTraining.ApplicationFrameworks_MainEmployer);
                    await _qnaApiClient.ResetPageAnswersBySequenceAndSectionNumber(applicationId, 6, 2, RoatpWorkflowPageIds.PlanningApprenticeshipTraining.OrganisationTransition_MainEmployer);
                    await _qnaApiClient.ResetPageAnswersBySequenceAndSectionNumber(applicationId, 6, 2, RoatpWorkflowPageIds.PlanningApprenticeshipTraining.OnlyDeliveringApprenticeshipFrameworks_MainEmployer);
                    break;
                case ApplicationRoute.EmployerProviderApplicationRoute:
                    await _qnaApiClient.ResetPageAnswersBySequenceAndSectionNumber(applicationId, 1, 4, RoatpWorkflowPageIds.DescribeYourOrganisation.MainSupportingStartPage);
                    await _qnaApiClient.ResetPageAnswersBySequenceAndSectionNumber(applicationId, 1, 4, RoatpWorkflowPageIds.DescribeYourOrganisation.EducationalInstituteTypeMainSupporting);
                    await _qnaApiClient.ResetPageAnswersBySequenceAndSectionNumber(applicationId, 1, 4, RoatpWorkflowPageIds.DescribeYourOrganisation.PublicBodyTypeMainSupporting);
                    await _qnaApiClient.ResetPageAnswersBySequenceAndSectionNumber(applicationId, 1, 4, RoatpWorkflowPageIds.DescribeYourOrganisation.SchoolMainSupporting);
                    await _qnaApiClient.ResetPageAnswersBySequenceAndSectionNumber(applicationId, 1, 4, RoatpWorkflowPageIds.DescribeYourOrganisation.RegisteredESFAMainSupporting);
                    await _qnaApiClient.ResetPageAnswersBySequenceAndSectionNumber(applicationId, 1, 4, RoatpWorkflowPageIds.DescribeYourOrganisation.FundedESFAMainSupporting);
                    await _qnaApiClient.ResetPageAnswersBySequenceAndSectionNumber(applicationId, 6, 2, RoatpWorkflowPageIds.PlanningApprenticeshipTraining.TypeOfApprenticeshipTraining_Main);
                    await _qnaApiClient.ResetPageAnswersBySequenceAndSectionNumber(applicationId, 6, 2, RoatpWorkflowPageIds.PlanningApprenticeshipTraining.TypeOfApprenticeshipTraining_Supporting);
                    await _qnaApiClient.ResetPageAnswersBySequenceAndSectionNumber(applicationId, 6, 2, RoatpWorkflowPageIds.PlanningApprenticeshipTraining.ApplicationFrameworks_Supporting);
                    await _qnaApiClient.ResetPageAnswersBySequenceAndSectionNumber(applicationId, 6, 2, RoatpWorkflowPageIds.PlanningApprenticeshipTraining.OrganisationTransition_Supporting);
                    await _qnaApiClient.ResetPageAnswersBySequenceAndSectionNumber(applicationId, 6, 2, RoatpWorkflowPageIds.PlanningApprenticeshipTraining.OnlyDeliveringApprenticeshipFrameworks_Supporting);
                    break;
            }
        
        }

        private async Task ResetSection14TasksForEmployerTags(Guid applicationId)
        {
            await _qnaApiClient.ResetPageAnswersBySequenceAndSectionNumber(applicationId, 1, 4, RoatpWorkflowPageIds.DescribeYourOrganisation.EmployerStartPage);
            await _qnaApiClient.ResetPageAnswersBySequenceAndSectionNumber(applicationId, 1, 4, RoatpWorkflowPageIds.DescribeYourOrganisation.EducationalInstituteTypeEmployer);
            await _qnaApiClient.ResetPageAnswersBySequenceAndSectionNumber(applicationId, 1, 4, RoatpWorkflowPageIds.DescribeYourOrganisation.PublicBodyTypeEmployer);
            await _qnaApiClient.ResetPageAnswersBySequenceAndSectionNumber(applicationId, 1, 4, RoatpWorkflowPageIds.DescribeYourOrganisation.SchoolEmployer);
            await _qnaApiClient.ResetPageAnswersBySequenceAndSectionNumber(applicationId, 1, 4, RoatpWorkflowPageIds.DescribeYourOrganisation.RegisteredESFAEmployer);
            await _qnaApiClient.ResetPageAnswersBySequenceAndSectionNumber(applicationId, 1, 4, RoatpWorkflowPageIds.DescribeYourOrganisation.FundedESFAEmployer);
        }
    }
}
