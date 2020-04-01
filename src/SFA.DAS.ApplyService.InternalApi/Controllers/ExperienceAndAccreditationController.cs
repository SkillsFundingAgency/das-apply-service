using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.InternalApi.Infrastructure;

namespace SFA.DAS.ApplyService.InternalApi.Controllers
{
    [Authorize]
    public class ExperienceAndAccreditationController : Controller
    {
        private readonly IInternalQnaApiClient _qnaApiClient;

        public ExperienceAndAccreditationController(IInternalQnaApiClient qnaApiClient)
        {
            _qnaApiClient = qnaApiClient;
        }

        [HttpGet("/Accreditation/{applicationId}/OfficeForStudents")]
        public async Task<string> GetOfficeForStudents(Guid applicationId)
        {
            return await _qnaApiClient.GetAnswerValue(applicationId,
                RoatpWorkflowSequenceIds.YourOrganisation,
                RoatpWorkflowSectionIds.YourOrganisation.ExperienceAndAccreditations,
                RoatpWorkflowPageIds.ExperienceAndAccreditations.OfficeForStudents,
                RoatpYourOrganisationQuestionIdConstants.OfficeForStudents
                );
        }

        [HttpGet("/Accreditation/{applicationId}/InitialTeacherTraining")]
        public async Task<string> GetInitialTeacherTraining(Guid applicationId)
        {
            return await _qnaApiClient.GetAnswerValue(applicationId,
                RoatpWorkflowSequenceIds.YourOrganisation,
                RoatpWorkflowSectionIds.YourOrganisation.ExperienceAndAccreditations,
                RoatpWorkflowPageIds.ExperienceAndAccreditations.InitialTeacherTraining,
                RoatpYourOrganisationQuestionIdConstants.InitialTeacherTraining);
        }

        [HttpGet("/Accreditation/{applicationId}/OfstedDetails")]
        public async Task<OfstedDetails> GetOfstedDetails(Guid applicationId)
        {
            var hasHadFullInspectionTask = _qnaApiClient.GetAnswerValue(applicationId,
                RoatpWorkflowSequenceIds.YourOrganisation,
                RoatpWorkflowSectionIds.YourOrganisation.ExperienceAndAccreditations,
                RoatpWorkflowPageIds.ExperienceAndAccreditations.HasHadFullInspection,
                RoatpYourOrganisationQuestionIdConstants.HasHadFullInspection);

            var receivedFullInspectionGradeForApprenticeshipsTask = _qnaApiClient.GetAnswerValue(applicationId,
                RoatpWorkflowSequenceIds.YourOrganisation,
                RoatpWorkflowSectionIds.YourOrganisation.ExperienceAndAccreditations,
                RoatpWorkflowPageIds.ExperienceAndAccreditations.ReceivedFullInspectionGradeForApprenticeships,
                RoatpYourOrganisationQuestionIdConstants.ReceivedFullInspectionGradeForApprenticeships);

            var fullInspectionOverallEffectivenessGradeTask = _qnaApiClient.GetAnswerValue(applicationId,
                RoatpWorkflowSequenceIds.YourOrganisation,
                RoatpWorkflowSectionIds.YourOrganisation.ExperienceAndAccreditations,
                RoatpWorkflowPageIds.ExperienceAndAccreditations.FullInspectionOverallEffectivenessGrade,
                RoatpYourOrganisationQuestionIdConstants.FullInspectionOverallEffectivenessGrade);

            var hasHadMonitoringVisitTask = _qnaApiClient.GetAnswerValue(applicationId,
                RoatpWorkflowSequenceIds.YourOrganisation,
                RoatpWorkflowSectionIds.YourOrganisation.ExperienceAndAccreditations,
                RoatpWorkflowPageIds.ExperienceAndAccreditations.HasHadMonitoringVisit,
                RoatpYourOrganisationQuestionIdConstants.HasHadMonitoringVisit);

            var hasMaintainedFundingSinceInspectionTask = _qnaApiClient.GetAnswerValue(applicationId,
                RoatpWorkflowSequenceIds.YourOrganisation,
                RoatpWorkflowSectionIds.YourOrganisation.ExperienceAndAccreditations,
                RoatpWorkflowPageIds.ExperienceAndAccreditations.MasMaintainedFundingSinceInspection,
                RoatpYourOrganisationQuestionIdConstants.MasMaintainedFundingSinceInspection);

            var hasHadShortInspectionWithinLast3YearsTask = _qnaApiClient.GetAnswerValue(applicationId,
                RoatpWorkflowSequenceIds.YourOrganisation,
                RoatpWorkflowSectionIds.YourOrganisation.ExperienceAndAccreditations,
                RoatpWorkflowPageIds.ExperienceAndAccreditations.HasHadShortInspectionWithinLast3Years,
                RoatpYourOrganisationQuestionIdConstants.HasHadShortInspectionWithinLast3Years);

            var hasMaintainedFullGradeInShortInspectionTask = _qnaApiClient.GetAnswerValue(applicationId,
                RoatpWorkflowSequenceIds.YourOrganisation,
                RoatpWorkflowSectionIds.YourOrganisation.ExperienceAndAccreditations,
                RoatpWorkflowPageIds.ExperienceAndAccreditations.HasMaintainedFullGradeInShortInspection,
                RoatpYourOrganisationQuestionIdConstants.HasMaintainedFullGradeInShortInspection);

            var fullInspectionApprenticeshipGradeTask = _qnaApiClient.GetAnswerValueFromActiveQuestion(applicationId,
                RoatpWorkflowSequenceIds.YourOrganisation,
                RoatpWorkflowSectionIds.YourOrganisation.ExperienceAndAccreditations,
                new PageAndQuestion(RoatpWorkflowPageIds.ExperienceAndAccreditations.FullInspectionApprenticeshipGradeNonOfsFunded, RoatpYourOrganisationQuestionIdConstants.FullInspectionApprenticeshipGradeOfsFunded),
                new PageAndQuestion(RoatpWorkflowPageIds.ExperienceAndAccreditations.FullInspectionApprenticeshipGradeOfsFunded, RoatpYourOrganisationQuestionIdConstants.FullInspectionApprenticeshipGradeNonOfsFunded));

            var gradeWithinLast3YearsTask = _qnaApiClient.GetAnswerValueFromActiveQuestion(applicationId,
                RoatpWorkflowSequenceIds.YourOrganisation,
                RoatpWorkflowSectionIds.YourOrganisation.ExperienceAndAccreditations,
                new PageAndQuestion(RoatpWorkflowPageIds.ExperienceAndAccreditations.GradeWithinLast3YearsOfsFunded, RoatpYourOrganisationQuestionIdConstants.GradeWithinLast3YearsOfsFunded),
                new PageAndQuestion(RoatpWorkflowPageIds.ExperienceAndAccreditations.GradeWithinLast3YearsNonOfsFunded, RoatpYourOrganisationQuestionIdConstants.GradeWithinLast3YearsNonOfsFunded));

            await Task.WhenAll(hasHadFullInspectionTask, receivedFullInspectionGradeForApprenticeshipsTask,
                fullInspectionOverallEffectivenessGradeTask, hasMaintainedFundingSinceInspectionTask,
                hasHadShortInspectionWithinLast3YearsTask, hasMaintainedFullGradeInShortInspectionTask,
                hasHadMonitoringVisitTask, fullInspectionApprenticeshipGradeTask, gradeWithinLast3YearsTask);

            return new OfstedDetails
            {
                HasHadFullInspection = hasHadFullInspectionTask.Result.ToUpper() == "YES",
                ReceivedFullInspectionGradeForApprenticeships = receivedFullInspectionGradeForApprenticeshipsTask.Result.ToUpper() == "YES",
                FullInspectionOverallEffectivenessGrade = fullInspectionOverallEffectivenessGradeTask.Result,
                MasMaintainedFundingSinceInspection = hasMaintainedFundingSinceInspectionTask.Result.ToUpper() == "YES",
                HasHadShortInspectionWithinLast3Years = hasHadShortInspectionWithinLast3YearsTask.Result.ToUpper() == "YES",
                HasMaintainedFullGradeInShortInspection = hasMaintainedFullGradeInShortInspectionTask.Result.ToUpper() == "YES",
                HasHadMonitoringVisit = hasHadMonitoringVisitTask.Result.ToUpper() == "YES",
                FullInspectionApprenticeshipGrade = fullInspectionApprenticeshipGradeTask.Result,
                GradeWithinTheLast3Years = gradeWithinLast3YearsTask.Result.ToUpper() == "YES"
            };
        }
    }
}
