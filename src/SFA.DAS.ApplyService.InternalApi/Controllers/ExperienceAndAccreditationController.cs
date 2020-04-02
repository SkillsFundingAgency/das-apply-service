using System;
using System.Linq;
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

            return new SubcontractorDeclaration
            {
                HasDeliveredTrainingAsSubcontractor = hasDeliveredTrainingAsSubcontractorTask.Result.ToUpper() == "YES",
                ContractFileName = contactFileNameTask.Result
            };
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
    }
}
