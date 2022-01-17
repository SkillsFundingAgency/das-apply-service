using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApplyService.Web.Infrastructure;

namespace SFA.DAS.ApplyService.Web.Controllers.Roatp
{
    [Authorize]
    public class ApplicationController: Controller
    {
        private readonly IQnaApiClient _qnaApiClient;

        public ApplicationController(IQnaApiClient qnaApiClient)
        {
            _qnaApiClient = qnaApiClient;
        }

        [Authorize(Policy = "AccessApplication")]
        public async Task<IActionResult> Download(Guid Id, int sequenceNo, int sectionId, string pageId, string questionId, string filename)
        {
            var selectedSection = await _qnaApiClient.GetSectionBySectionNo(Id, sequenceNo, sectionId);

            var response = await _qnaApiClient.DownloadFile(Id, selectedSection.Id, pageId, questionId, filename);
            var fileStream = await response.Content.ReadAsStreamAsync();

            return File(fileStream, response.Content.Headers.ContentType.MediaType, filename);

        }

        [Authorize(Policy = "AccessInProgressApplication")]
        public async Task<IActionResult> DeleteFile(Guid Id, int sequenceNo, int sectionId, string pageId, string questionId, string filename, string __redirectAction)
        {
            var selectedSection = await _qnaApiClient.GetSectionBySectionNo(Id, sequenceNo, sectionId);

            await _qnaApiClient.DeleteFile(Id, selectedSection.Id, pageId, questionId, filename);

            return RedirectToAction("Page", "RoatpApplication", new { applicationId = Id, sequenceId = sequenceNo, sectionId, pageId, redirectAction = __redirectAction });
        }
    }
}
