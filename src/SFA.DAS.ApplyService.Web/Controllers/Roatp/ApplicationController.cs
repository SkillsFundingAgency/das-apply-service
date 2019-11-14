using System;
using System.Linq;
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
        private readonly IApplicationApiClient _apiClient;

        public ApplicationController(IQnaApiClient qnaApiClient, IApplicationApiClient apiClient)
        {
            _qnaApiClient = qnaApiClient;
            _apiClient = apiClient;
        }

        public async Task<IActionResult> Download(Guid Id, int sequenceNo, int sectionId, string pageId, string questionId, string filename)
        {
            var sequences = await _qnaApiClient.GetSequences(Id);
            var selectedSequence = sequences.Single(x => x.SequenceId == sequenceNo);
            var sections = await _qnaApiClient.GetSections(Id, selectedSequence.Id);
            var selectedSection = sections.Single(x => x.SectionId == sectionId);
            var response = await _qnaApiClient.DownloadFile(Id, selectedSection.Id, pageId, questionId, filename);
            var fileStream = await response.Content.ReadAsStreamAsync();

            return File(fileStream, response.Content.Headers.ContentType.MediaType, filename);

        }


        public async Task<IActionResult> DeleteFile(Guid Id, int sequenceNo, int sectionId, string pageId, string questionId, string filename, string redirectAction)
        {
            var applicationId = Id;
            var sequences = await _qnaApiClient.GetSequences(applicationId);
            var selectedSequence = sequences.Single(x => x.SequenceId == sequenceNo);
            var sections = await _qnaApiClient.GetSections(applicationId, selectedSequence.Id);
            var selectedSection = sections.Single(x => x.SectionId == sectionId);


            await _apiClient.RemoveSectionCompleted(applicationId, selectedSection.Id);
            await _qnaApiClient.DeleteFile(applicationId, selectedSection.Id, pageId, questionId, filename);

            return RedirectToAction("Page", "RoatpApplication", new { applicationId, sequenceId = sequenceNo, sectionId, pageId, redirectAction });
        }
    }
}
