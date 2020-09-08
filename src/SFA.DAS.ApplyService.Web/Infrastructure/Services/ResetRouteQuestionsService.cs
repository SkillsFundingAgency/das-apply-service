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
            var sections = await _qnaApiClient.GetSections(applicationId);
            var section14Id = sections.FirstOrDefault(x => x.SequenceId == 1 && x.SectionId == 4)?.Id;
            var section62Id = sections.FirstOrDefault(x => x.SequenceId == 6 && x.SectionId == 2)?.Id;
            if (section14Id == null || section62Id == null)
                return;

            switch (routeId)
            {
                case ApplicationRoute.MainProviderApplicationRoute:
                    await ResetSection14TasksForEmployerTags(applicationId, section14Id.Value);
                    await _qnaApiClient.ResetPageAnswers(applicationId, section62Id.Value, RoatpWorkflowPageIds.PlanningApprenticeshipTraining.TypeOfApprenticeshipTraining_Employer);
                    await _qnaApiClient.ResetPageAnswers(applicationId, section62Id.Value, RoatpWorkflowPageIds.PlanningApprenticeshipTraining.TypeOfApprenticeshipTraining_Supporting);
                    await _qnaApiClient.ResetPageAnswers(applicationId, section62Id.Value, RoatpWorkflowPageIds.PlanningApprenticeshipTraining.ApplicationFrameworks_Supporting);
                    await _qnaApiClient.ResetPageAnswers(applicationId, section62Id.Value, RoatpWorkflowPageIds.PlanningApprenticeshipTraining.OrganisationTransition_Supporting);
                    await _qnaApiClient.ResetPageAnswers(applicationId, section62Id.Value, RoatpWorkflowPageIds.PlanningApprenticeshipTraining.OnlyDeliveringApprenticeshipFrameworks_Supporting);
                    break;
                case ApplicationRoute.SupportingProviderApplicationRoute:
                    await ResetSection14TasksForEmployerTags(applicationId, section14Id.Value);
                    await _qnaApiClient.ResetPageAnswers(applicationId, section62Id.Value, RoatpWorkflowPageIds.PlanningApprenticeshipTraining.TypeOfApprenticeshipTraining_Main);
                    await _qnaApiClient.ResetPageAnswers(applicationId, section62Id.Value, RoatpWorkflowPageIds.PlanningApprenticeshipTraining.TypeOfApprenticeshipTraining_Employer);
                    await _qnaApiClient.ResetPageAnswers(applicationId, section62Id.Value, RoatpWorkflowPageIds.PlanningApprenticeshipTraining.ApplicationFrameworks_MainEmployer);
                    await _qnaApiClient.ResetPageAnswers(applicationId, section62Id.Value, RoatpWorkflowPageIds.PlanningApprenticeshipTraining.OrganisationTransition_MainEmployer); 
                    await _qnaApiClient.ResetPageAnswers(applicationId, section62Id.Value, RoatpWorkflowPageIds.PlanningApprenticeshipTraining.OnlyDeliveringApprenticeshipFrameworks_MainEmployer);
                    break;
                case ApplicationRoute.EmployerProviderApplicationRoute:
                    await _qnaApiClient.ResetPageAnswers(applicationId, section14Id.Value, RoatpWorkflowPageIds.DescribeYourOrganisation.MainSupportingStartPage);
                    await _qnaApiClient.ResetPageAnswers(applicationId, section14Id.Value, RoatpWorkflowPageIds.DescribeYourOrganisation.EducationalInstituteTypeMainSupporting);
                    await _qnaApiClient.ResetPageAnswers(applicationId, section14Id.Value, RoatpWorkflowPageIds.DescribeYourOrganisation.PublicBodyTypeMainSupporting);
                    await _qnaApiClient.ResetPageAnswers(applicationId, section14Id.Value, RoatpWorkflowPageIds.DescribeYourOrganisation.SchoolMainSupporting);
                    await _qnaApiClient.ResetPageAnswers(applicationId, section14Id.Value, RoatpWorkflowPageIds.DescribeYourOrganisation.RegisteredESFAMainSupporting);
                    await _qnaApiClient.ResetPageAnswers(applicationId, section14Id.Value, RoatpWorkflowPageIds.DescribeYourOrganisation.FundedESFAMainSupporting);
                    await _qnaApiClient.ResetPageAnswers(applicationId, section62Id.Value, RoatpWorkflowPageIds.PlanningApprenticeshipTraining.TypeOfApprenticeshipTraining_Main);
                    await _qnaApiClient.ResetPageAnswers(applicationId, section62Id.Value, RoatpWorkflowPageIds.PlanningApprenticeshipTraining.TypeOfApprenticeshipTraining_Supporting);
                    await _qnaApiClient.ResetPageAnswers(applicationId, section62Id.Value, RoatpWorkflowPageIds.PlanningApprenticeshipTraining.ApplicationFrameworks_Supporting);
                    await _qnaApiClient.ResetPageAnswers(applicationId, section62Id.Value, RoatpWorkflowPageIds.PlanningApprenticeshipTraining.OrganisationTransition_Supporting);
                    await _qnaApiClient.ResetPageAnswers(applicationId, section62Id.Value, RoatpWorkflowPageIds.PlanningApprenticeshipTraining.OnlyDeliveringApprenticeshipFrameworks_Supporting);
                    break;
            }
        
        }

        private async Task ResetSection14TasksForEmployerTags(Guid applicationId, Guid section14Id)
        {
            await _qnaApiClient.ResetPageAnswers(applicationId, section14Id, RoatpWorkflowPageIds.DescribeYourOrganisation.EmployerStartPage);
            await _qnaApiClient.ResetPageAnswers(applicationId, section14Id, RoatpWorkflowPageIds.DescribeYourOrganisation.EducationalInstituteTypeEmployer);
            await _qnaApiClient.ResetPageAnswers(applicationId, section14Id, RoatpWorkflowPageIds.DescribeYourOrganisation.PublicBodyTypeEmployer);
            await _qnaApiClient.ResetPageAnswers(applicationId, section14Id, RoatpWorkflowPageIds.DescribeYourOrganisation.SchoolEmployer);
            await _qnaApiClient.ResetPageAnswers(applicationId, section14Id, RoatpWorkflowPageIds.DescribeYourOrganisation.RegisteredESFAEmployer);
            await _qnaApiClient.ResetPageAnswers(applicationId, section14Id, RoatpWorkflowPageIds.DescribeYourOrganisation.FundedESFAEmployer);
        }
    }
}
