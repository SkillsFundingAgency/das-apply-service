using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Application.Apply.Clarification;
using SFA.DAS.ApplyService.Domain.Apply.Clarification;
using SFA.DAS.ApplyService.InternalApi.Services.Assessor;
using SFA.DAS.ApplyService.InternalApi.Services.Files;
using SFA.DAS.ApplyService.InternalApi.Types.Assessor;

namespace SFA.DAS.ApplyService.InternalApi.Controllers
{
    [Authorize]
    public class RoatpClarificationController : Controller
    {
        private readonly ILogger<RoatpClarificationController> _logger;
        private readonly IMediator _mediator;
        private readonly IAssessorSequenceService _sequenceService;
        private readonly IAssessorPageService _pageService;
        private readonly IAssessorSectorService _sectorService;
        private readonly IAssessorSectorDetailsService _sectorDetailsService;
        private readonly IFileStorageService _fileStorageService;

        public RoatpClarificationController(ILogger<RoatpClarificationController> logger, IMediator mediator,
            IAssessorSequenceService assessorSequenceService, IAssessorPageService assessorPageService,
            IAssessorSectorService assessorSectorService, IAssessorSectorDetailsService assessorSectorDetailsService,
            IFileStorageService fileStorageService)
        {
            _logger = logger;
            _mediator = mediator;
            _sequenceService = assessorSequenceService;
            _pageService = assessorPageService;
            _sectorService = assessorSectorService;
            _sectorDetailsService = assessorSectorDetailsService;
            _fileStorageService = fileStorageService;
        }

        [HttpGet("Clarification/Applications/{applicationId}/Overview")]
        public async Task<List<AssessorSequence>> GetClarificationOverview(Guid applicationId)
        {
            var overviewSequences = await _sequenceService.GetSequences(applicationId);

            return overviewSequences.OrderBy(seq => seq.SequenceNumber).ToList();
        }

        [HttpPost("Clarification/Applications/{applicationId}/Sectors")]
        public async Task<List<AssessorSector>> GetSectors(Guid applicationId, [FromBody] GetSectorsRequest request)
        {
            var sectors = await _sectorService.GetSectorsForClarification(applicationId, request.UserId);

            return sectors.OrderBy(sec => sec.PageId).ToList();
        }

        [HttpGet("Clarification/Applications/{applicationId}/SectorDetails/{pageId}")]
        public async Task<AssessorSectorDetails> GetSectorDetails(Guid applicationId, string pageId)
        {
            return await _sectorDetailsService.GetSectorDetails(applicationId, pageId);
        }

        [HttpGet("Clarification/Applications/{applicationId}/Sequences/{sequenceNumber}/Sections/{sectionNumber}/Page")]
        public async Task<AssessorPage> GetFirstClarificationPage(Guid applicationId, int sequenceNumber, int sectionNumber)
        {
            return await GetClarificationPage(applicationId, sequenceNumber, sectionNumber, null);
        }

        [HttpGet("Clarification/Applications/{applicationId}/Sequences/{sequenceNumber}/Sections/{sectionNumber}/Page/{pageId}")]
        public async Task<AssessorPage> GetClarificationPage(Guid applicationId, int sequenceNumber, int sectionNumber, string pageId)
        {
            return await _pageService.GetPage(applicationId, sequenceNumber, sectionNumber, pageId);
        }

        [HttpPost("Clarification/Applications/{applicationId}/SubmitPageReviewOutcome")]
        public async Task<IActionResult> SubmitPageReviewOutcome(Guid applicationId, [FromForm] SubmitPageReviewOutcomeCommand request)
        {
            string clarificationFile = null;

            if (Request.Form.Files != null && Request.Form.Files.Any())
            {
                clarificationFile = Request.Form.Files.First().FileName;
                var uploadedSuccessfully = await _fileStorageService.UploadFiles(applicationId, request.SequenceNumber, request.SectionNumber, request.PageId, Request.Form.Files, ContainerType.Assessor, new CancellationToken());

                if (!uploadedSuccessfully)
                {
                    _logger.LogError($"Unable to upload files for application: {applicationId} || pageId {request.PageId}");
                    return BadRequest();
                }
            }

            await _mediator.Send(new SubmitClarificationPageOutcomeRequest(applicationId, request.SequenceNumber, request.SectionNumber, request.PageId, request.UserId, request.UserName, request.Status, request.Comment, request.ClarificationResponse, clarificationFile));
            return Ok();
        }

        [HttpPost("Clarification/Applications/{applicationId}/GetPageReviewOutcome")]
        public async Task<ClarificationPageReviewOutcome> GetPageReviewOutcome(Guid applicationId, [FromBody] GetPageReviewOutcomeRequest request)
        {
            var pageReviewOutcome = await _mediator.Send(new GetClarificationPageReviewOutcomeRequest(applicationId, request.SequenceNumber, request.SectionNumber, request.PageId, request.UserId));

            return pageReviewOutcome;
        }

        [HttpPost("Clarification/Applications/{applicationId}/GetPageReviewOutcomesForSection")]
        public async Task<List<ClarificationPageReviewOutcome>> GetPageReviewOutcomesForSection(Guid applicationId, [FromBody] GetPageReviewOutcomesForSectionRequest request)
        {
            var reviewOutcomes = await _mediator.Send(new GetClarificationPageReviewOutcomesForSectionRequest(applicationId, request.SequenceNumber, request.SectionNumber, request.UserId));

            return reviewOutcomes;
        }

        [HttpPost("Clarification/Applications/{applicationId}/GetAllPageReviewOutcomes")]
        public async Task<List<ClarificationPageReviewOutcome>> GetAllPageReviewOutcomes(Guid applicationId, [FromBody] GetAllPageReviewOutcomesRequest request)
        {
            var reviewOutcomes = await _mediator.Send(new GetAllClarificationPageReviewOutcomesRequest(applicationId, request.UserId));

            return reviewOutcomes;
        }

        [HttpGet("Clarification/Applications/{applicationId}/Sequences/{sequenceNumber}/Sections/{sectionNumber}/Page/{pageId}/Download/{fileName}")]
        public async Task<IActionResult> DownloadClarificationFile(Guid applicationId, int sequenceNumber, int sectionNumber, string pageId, string fileName)
        {
            var file = await _fileStorageService.DownloadFile(applicationId, sequenceNumber, sectionNumber, pageId, fileName, ContainerType.Assessor, new CancellationToken());

            if (file is null)
            {
                _logger.LogError($"Unable to download file for application: {applicationId} || pageId {pageId} || filename {fileName}");
                return NotFound();
            }

            return File(file.Stream, file.ContentType, file.FileName);
        }

        [HttpDelete("Clarification/Applications/{applicationId}/Sequences/{sequenceNumber}/Sections/{sectionNumber}/Page/{pageId}/Delete/{fileName}")]
        public async Task<bool> DeleteClarificationFile(Guid applicationId, int sequenceNumber, int sectionNumber, string pageId, string fileName)
        {
            await _mediator.Send(new DeleteClarificationFileRequest(applicationId, sequenceNumber, sectionNumber, pageId, fileName));

            var deletedSuccessfully = await _fileStorageService.DeleteFile(applicationId, sequenceNumber, sectionNumber, pageId, fileName, ContainerType.Assessor, new CancellationToken());

            if (!deletedSuccessfully)
            {
                _logger.LogError($"Unable to delete file for application: {applicationId} || pageId {pageId} || filename {fileName}");
            }

            return deletedSuccessfully;
        }

        public class SubmitPageReviewOutcomeCommand
        {
            public int SequenceNumber { get; set; }
            public int SectionNumber { get; set; }
            public string PageId { get; set; }
            public string UserId { get; set; }
            public string UserName { get; set; }
            public string Status { get; set; }
            public string Comment { get; set; }
            public string ClarificationResponse { get; set; }
        }

        public class GetPageReviewOutcomeRequest
        {
            public int SequenceNumber { get; set; }
            public int SectionNumber { get; set; }
            public string PageId { get; set; }
            public string UserId { get; set; }
        }

        public class GetPageReviewOutcomesForSectionRequest
        {
            public int SequenceNumber { get; set; }
            public int SectionNumber { get; set; }
            public string UserId { get; set; }
        }

        public class GetAllPageReviewOutcomesRequest
        {
            public string UserId { get; set; }
        }

        public class GetSectorsRequest
        {
            public string UserId { get; set; }
        }
    }
}
