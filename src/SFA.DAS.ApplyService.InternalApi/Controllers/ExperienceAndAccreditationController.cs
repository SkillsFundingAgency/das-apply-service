﻿using System;
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
        public async Task<InitialTeacherTraining> GetInitialTeacherTraining(Guid applicationId)
        {
            var initialTeacherTrainingTask = _qnaApiClient.GetAnswerValue(applicationId,
                RoatpWorkflowSequenceIds.YourOrganisation,
                RoatpWorkflowSectionIds.YourOrganisation.ExperienceAndAccreditations,
                RoatpWorkflowPageIds.ExperienceAndAccreditations.InitialTeacherTraining,
                RoatpYourOrganisationQuestionIdConstants.InitialTeacherTraining);

            var isPostGradTrainingOnlyApprenticeshipTask = _qnaApiClient.GetAnswerValue(applicationId,
                RoatpWorkflowSequenceIds.YourOrganisation,
                RoatpWorkflowSectionIds.YourOrganisation.ExperienceAndAccreditations,
                RoatpWorkflowPageIds.ExperienceAndAccreditations.IsPostGradTrainingOnlyApprenticeship,
                RoatpYourOrganisationQuestionIdConstants.IsPostGradTrainingOnlyApprenticeship);

            await Task.WhenAll(initialTeacherTrainingTask, isPostGradTrainingOnlyApprenticeshipTask);

            return new InitialTeacherTraining
            {
                DoesOrganisationOfferInitialTeacherTraining = initialTeacherTrainingTask.Result.ToUpper() == "YES",
                IsPostGradOnlyApprenticeship = isPostGradTrainingOnlyApprenticeshipTask.Result.ToUpper() == "YES"
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
                RoatpWorkflowPageIds.ExperienceAndAccreditations.HasMaintainedFundingSinceInspection,
                RoatpYourOrganisationQuestionIdConstants.HasMaintainedFundingSinceInspection);

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
                HasHadFullInspection = hasHadFullInspectionTask.Result?.ToUpper() == "YES",
                ReceivedFullInspectionGradeForApprenticeships = receivedFullInspectionGradeForApprenticeshipsTask.Result?.ToUpper() == "YES",
                FullInspectionOverallEffectivenessGrade = fullInspectionOverallEffectivenessGradeTask.Result,
                HasMaintainedFundingSinceInspection = hasMaintainedFundingSinceInspectionTask.Result?.ToUpper() == "YES",
                HasHadShortInspectionWithinLast3Years = hasHadShortInspectionWithinLast3YearsTask.Result?.ToUpper() == "YES",
                HasMaintainedFullGradeInShortInspection = hasMaintainedFullGradeInShortInspectionTask.Result?.ToUpper() == "YES",
                HasHadMonitoringVisit = hasHadMonitoringVisitTask.Result?.ToUpper() == "YES",
                FullInspectionApprenticeshipGrade = fullInspectionApprenticeshipGradeTask.Result,
                GradeWithinTheLast3Years = gradeWithinLast3YearsTask.Result?.ToUpper() == "YES"
            };
        }
    }
}
