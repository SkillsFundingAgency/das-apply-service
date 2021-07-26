using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.InternalApi.Infrastructure;
using SFA.DAS.ApplyService.InternalApi.Services.Files;

namespace SFA.DAS.ApplyService.InternalApi.Controllers
{
    [Authorize]
    public class ExperienceAndAccreditationController : Controller
    {
        private readonly IInternalQnaApiClient _qnaApiClient;
        private readonly IFileStorageService _fileStorageService;
        public ExperienceAndAccreditationController(IInternalQnaApiClient qnaApiClient, IFileStorageService fileStorageService)
        {
            _qnaApiClient = qnaApiClient;
            _fileStorageService = fileStorageService;
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
        public async Task<InitialTeacherTraining> GetInitialTeacherTraining(Guid applicationId)
        {
            var initialTeacherTraining = await _qnaApiClient.GetAnswerValue(applicationId,
                RoatpWorkflowSequenceIds.YourOrganisation,
                RoatpWorkflowSectionIds.YourOrganisation.ExperienceAndAccreditations,
                RoatpWorkflowPageIds.ExperienceAndAccreditations.InitialTeacherTraining,
                RoatpYourOrganisationQuestionIdConstants.InitialTeacherTraining);

            if (initialTeacherTraining?.ToUpper() == "YES")
            {
                var isPostGradTrainingOnlyApprenticeship = await _qnaApiClient.GetAnswerValue(applicationId,
                    RoatpWorkflowSequenceIds.YourOrganisation,
                    RoatpWorkflowSectionIds.YourOrganisation.ExperienceAndAccreditations,
                    RoatpWorkflowPageIds.ExperienceAndAccreditations.IsPostGradTrainingOnlyApprenticeship,
                    RoatpYourOrganisationQuestionIdConstants.IsPostGradTrainingOnlyApprenticeship);

                return new InitialTeacherTraining
                {
                    DoesOrganisationOfferInitialTeacherTraining = true,
                    IsPostGradOnlyApprenticeship = isPostGradTrainingOnlyApprenticeship.ToUpper() == "YES"
                };
            }

            return new InitialTeacherTraining
            {
                DoesOrganisationOfferInitialTeacherTraining = false,
                IsPostGradOnlyApprenticeship = null
            };
        }

        [HttpGet("/Accreditation/{applicationId}/SubcontractDeclaration")]
        public async Task<SubcontractorDeclaration> GetSubcontractorDeclaration(Guid applicationId)
        {
            var hasDeliveredTrainingAsSubcontractorTask = _qnaApiClient.GetAnswerValue(applicationId,
                RoatpWorkflowSequenceIds.YourOrganisation,
                RoatpWorkflowSectionIds.YourOrganisation.ExperienceAndAccreditations,
                RoatpWorkflowPageIds.ExperienceAndAccreditations.SubcontractorDeclaration,
                RoatpYourOrganisationQuestionIdConstants.HasDeliveredTrainingAsSubcontractor
            );

            var contactFileNameTask = _qnaApiClient.GetAnswerValue(applicationId,
                RoatpWorkflowSequenceIds.YourOrganisation,
                RoatpWorkflowSectionIds.YourOrganisation.ExperienceAndAccreditations,
                RoatpWorkflowPageIds.ExperienceAndAccreditations.SubcontractorContractFile,
                RoatpYourOrganisationQuestionIdConstants.ContractFileName
            );

            await Task.WhenAll(hasDeliveredTrainingAsSubcontractorTask, contactFileNameTask);

            var result = new SubcontractorDeclaration
            {
                HasDeliveredTrainingAsSubcontractor = hasDeliveredTrainingAsSubcontractorTask.Result?.ToUpper() == "YES",
                ContractFileName = contactFileNameTask.Result
            };

            return result;
        }

        [HttpGet("/Accreditation/{applicationId}/SubcontractDeclaration/ContractFile")]
        public async Task<FileStreamResult> GetSubcontractorDeclarationContractFile(Guid applicationId)
        {
            return await _qnaApiClient.GetDownloadFile(applicationId,
                RoatpWorkflowSequenceIds.YourOrganisation,
                RoatpWorkflowSectionIds.YourOrganisation.ExperienceAndAccreditations,
                RoatpWorkflowPageIds.ExperienceAndAccreditations.SubcontractorContractFile,
                RoatpYourOrganisationQuestionIdConstants.ContractFileName);
        }

        [HttpGet("/Accreditation/{applicationId}/SubcontractDeclaration/ContractFileClarification/{fileName}")]
        public async Task<IActionResult> GetSubcontractorDeclarationContractFileClarification(Guid applicationId, string fileName)
        {
            var file= await _fileStorageService.DownloadFile(applicationId,
                RoatpWorkflowSequenceIds.YourOrganisation,
                RoatpWorkflowSectionIds.YourOrganisation.ExperienceAndAccreditations,
                RoatpClarificationUpload.SubcontractorDeclarationClarificationFile, fileName,
                ContainerType.Gateway, new CancellationToken());

            if (file is null)
            {
                return NotFound();
            }

            return File(file.Stream, file.ContentType, file.FileName);
        }

        [HttpGet("/Accreditation/{applicationId}/OfstedDetails")]
        public async Task<OfstedDetails> GetOfstedDetails(Guid applicationId)
        {
            var section = await _qnaApiClient.GetSectionBySectionNo(applicationId, RoatpWorkflowSequenceIds.YourOrganisation, RoatpWorkflowSectionIds.YourOrganisation.ExperienceAndAccreditations);

            var hasHadFullInspection = YesNoValueForPageQuestion(section,
                RoatpWorkflowPageIds.ExperienceAndAccreditations.HasHadFullInspection, 
                RoatpYourOrganisationQuestionIdConstants.HasHadFullInspection);

            var hasReceivedFullInspectionGradeForApprenticeships = YesNoValueForPageQuestion(section, 
                RoatpWorkflowPageIds.ExperienceAndAccreditations.ReceivedFullInspectionGradeForApprenticeships, 
                RoatpYourOrganisationQuestionIdConstants.ReceivedFullInspectionGradeForApprenticeships);

            var fullInspectionOverallEffectivenessGrade = ValueForPageQuestion(section, 
                RoatpWorkflowPageIds.ExperienceAndAccreditations.FullInspectionOverallEffectivenessGrade, 
                RoatpYourOrganisationQuestionIdConstants.FullInspectionOverallEffectivenessGrade);

            var hasHadMonitoringVisit = YesNoValueForPageQuestion(section, 
                RoatpWorkflowPageIds.ExperienceAndAccreditations.HasHadMonitoringVisit, 
                RoatpYourOrganisationQuestionIdConstants.HasHadMonitoringVisit);

            var has2MonitoringVisitsGradedInadequate= YesNoValueForPageQuestion(section, 
                RoatpWorkflowPageIds.ExperienceAndAccreditations.Has2MonitoringVisitsGradedInadequate, RoatpYourOrganisationQuestionIdConstants.Has2MonitoringVisitsGradedInadequate);

            var hasMonitoringVisitGradedInadequateInLast18Months = YesNoValueForPageQuestion(section, 
                RoatpWorkflowPageIds.ExperienceAndAccreditations.HasMonitoringVisitGradedInadequateInLast18Months, 
                RoatpYourOrganisationQuestionIdConstants.HasMonitoringVisitGradedInadequateInLast18Months);

            var hasMaintainedFundingSinceInspection = YesNoValueForPageQuestion(section, 
                RoatpWorkflowPageIds.ExperienceAndAccreditations.HasMaintainedFundingSinceInspection, 
                RoatpYourOrganisationQuestionIdConstants.HasMaintainedFundingSinceInspection);

            var hasHadShortInspectionWithinLast3Years = YesNoValueForPageQuestion(section, 
                RoatpWorkflowPageIds.ExperienceAndAccreditations.HasHadShortInspectionWithinLast3Years,
                RoatpYourOrganisationQuestionIdConstants.HasHadShortInspectionWithinLast3Years);

            var hasMaintainedFullGradeInShortInspection = YesNoValueForPageQuestion(section, 
                RoatpWorkflowPageIds.ExperienceAndAccreditations.HasMaintainedFullGradeInShortInspection, 
                RoatpYourOrganisationQuestionIdConstants.HasMaintainedFullGradeInShortInspection);

            var fullInspectionApprenticeshipGradeOfsFunded = ValueForPageQuestion(section, 
                RoatpWorkflowPageIds.ExperienceAndAccreditations.FullInspectionApprenticeshipGradeNonOfsFunded, 
                RoatpYourOrganisationQuestionIdConstants.FullInspectionApprenticeshipGradeOfsFunded);
            var fullInspectionApprenticeshipGradeNonOfsFunded = ValueForPageQuestion(section, 
                RoatpWorkflowPageIds.ExperienceAndAccreditations.FullInspectionApprenticeshipGradeOfsFunded, 
                RoatpYourOrganisationQuestionIdConstants.FullInspectionApprenticeshipGradeNonOfsFunded);

            var fullInspectionApprenticeshipGrade = fullInspectionApprenticeshipGradeNonOfsFunded ?? fullInspectionApprenticeshipGradeOfsFunded;

            var hasGradeWithinLast3YearsOfsFunded = YesNoValueForPageQuestion(section, 
                RoatpWorkflowPageIds.ExperienceAndAccreditations.GradeWithinLast3YearsOfsFunded, 
                RoatpYourOrganisationQuestionIdConstants.GradeWithinLast3YearsOfsFunded);
           
            var hasGradeWithinLast3YearsNonOfsFunded = YesNoValueForPageQuestion(section, 
                RoatpWorkflowPageIds.ExperienceAndAccreditations.GradeWithinLast3YearsNonOfsFunded, 
                RoatpYourOrganisationQuestionIdConstants.GradeWithinLast3YearsNonOfsFunded);

            var hasGradeWithinLast3Years = hasGradeWithinLast3YearsOfsFunded ?? hasGradeWithinLast3YearsNonOfsFunded;

            return new OfstedDetails
            {
                HasHadFullInspection = hasHadFullInspection,
                ReceivedFullInspectionGradeForApprenticeships = hasReceivedFullInspectionGradeForApprenticeships,
                FullInspectionOverallEffectivenessGrade = fullInspectionOverallEffectivenessGrade,
                HasMaintainedFundingSinceInspection = hasMaintainedFundingSinceInspection,
                HasHadShortInspectionWithinLast3Years = hasHadShortInspectionWithinLast3Years,
                HasMaintainedFullGradeInShortInspection = hasMaintainedFullGradeInShortInspection,
                HasHadMonitoringVisit = hasHadMonitoringVisit,
                Has2MonitoringVisitsGradedInadequate = has2MonitoringVisitsGradedInadequate,
                HasMonitoringVisitGradedInadequateInLast18Months = hasMonitoringVisitGradedInadequateInLast18Months,
                FullInspectionApprenticeshipGrade = fullInspectionApprenticeshipGrade,
                GradeWithinTheLast3Years = hasGradeWithinLast3Years
            };
        }

        private static bool? YesNoValueForPageQuestion(ApplicationSection section, string pageId, string questionId)
        {
            var value = ValueForPageQuestion(section, pageId, questionId);
            return value != null
                ? value.ToUpper() == "YES"
                : (
                    bool?) null;
        }

        private static string ValueForPageQuestion(ApplicationSection section, string pageId, string questionId)
        {
            var page = section?.QnAData?.Pages.FirstOrDefault(p =>
                p.Active && p.PageId == pageId);
            var pageOfAnswers = page?.PageOfAnswers;

            return pageOfAnswers == null 
                ? null 
                : (from answers in pageOfAnswers from answer in answers.Answers where answer.QuestionId == questionId select answer.Value).FirstOrDefault();
        }
    }
}
