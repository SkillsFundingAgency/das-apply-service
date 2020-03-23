using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.InternalApi.Infrastructure;

namespace SFA.DAS.ApplyService.InternalApi.Controllers
{
    [Authorize]
    public class GatewayExperienceAndAccreditationController : Controller
    {
        private readonly IInternalQnaApiClient _qnaApiClient;

        /// <summary>
        /// Returns trading name for an application
        /// </summary>
        /// <param name="qnaApiClient"></param>
        public GatewayExperienceAndAccreditationController(IInternalQnaApiClient qnaApiClient)
        {
            _qnaApiClient = qnaApiClient;
        }

        [HttpGet("/Gateway/{applicationId}/OfficeForStudent")]
        public async Task<string> GetOfficeForStudents(Guid applicationId)
        {
            var websiteNamePage = await _qnaApiClient.GetPageBySectionNo(applicationId, 
                RoatpWorkflowSequenceIds.YourOrganisation, 
                RoatpWorkflowSectionIds.YourOrganisation.ExperienceAndAccreditations, 
                RoatpWorkflowPageIds.ExperienceAndAccreditations.OfficeForStudents);

            return websiteNamePage?.PageOfAnswers?.SelectMany(a => a.Answers)?
                .FirstOrDefault(a => a.QuestionId == RoatpYourOrganisationQuestionIdConstants.OfficeForStudents)?.Value;
        }

        [HttpGet("/Gateway/{applicationId}/InitialTeacherTraining")]
        public async Task<string> GetInitialTeacherTraining(Guid applicationId)
        {
            var websiteNamePage = await _qnaApiClient.GetPageBySectionNo(applicationId,
                RoatpWorkflowSequenceIds.YourOrganisation,
                RoatpWorkflowSectionIds.YourOrganisation.ExperienceAndAccreditations,
                RoatpWorkflowPageIds.ExperienceAndAccreditations.InititalTeacherTraining);

            return websiteNamePage?.PageOfAnswers?.SelectMany(a => a.Answers)?
                .FirstOrDefault(a => a.QuestionId == RoatpYourOrganisationQuestionIdConstants.InitialTeacherTraining)?.Value;
        }
    }
}
