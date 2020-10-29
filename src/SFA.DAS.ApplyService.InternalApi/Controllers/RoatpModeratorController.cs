using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApplyService.Application.Apply.Moderator;
using SFA.DAS.ApplyService.Domain.Apply.Moderator;
using SFA.DAS.ApplyService.InternalApi.Services.Assessor;
using SFA.DAS.ApplyService.InternalApi.Types.Assessor;

namespace SFA.DAS.ApplyService.InternalApi.Controllers
{
    [Authorize]
    public class RoatpModeratorController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IAssessorSequenceService _sequenceService;
        private readonly IAssessorPageService _pageService;
        private readonly IAssessorSectorService _sectorService;
        private readonly IAssessorSectorDetailsService _sectorDetailsService;

        public RoatpModeratorController(IMediator mediator,
            IAssessorSequenceService assessorSequenceService, IAssessorPageService assessorPageService,
            IAssessorSectorService assessorSectorService, IAssessorSectorDetailsService assessorSectorDetailsService)
        {
            _mediator = mediator;
            _sequenceService = assessorSequenceService;
            _pageService = assessorPageService;
            _sectorService = assessorSectorService;
            _sectorDetailsService = assessorSectorDetailsService;
        }

        [HttpGet("Moderator/Applications/{applicationId}/Overview")]
        public async Task<List<AssessorSequence>> GetModeratorOverview(Guid applicationId)
        {
            var overviewSequences = await _sequenceService.GetSequences(applicationId);

            return overviewSequences.OrderBy(seq => seq.SequenceNumber).ToList();
        }

        [HttpPost("Moderator/Applications/{applicationId}/Sectors")]
        public async Task<List<AssessorSector>> GetSectors(Guid applicationId, [FromBody] GetSectorsRequest request)
        {
            var sectors = await _sectorService.GetSectorsForModerator(applicationId, request.UserId);

            return sectors.OrderBy(sec => sec.PageId).ToList();
        }

        [HttpGet("Moderator/Applications/{applicationId}/SectorDetails/{pageId}")]
        public async Task<AssessorSectorDetails> GetSectorDetails(Guid applicationId, string pageId)
        {
            return await _sectorDetailsService.GetSectorDetails(applicationId, pageId);
        }

        [HttpGet("Moderator/Applications/{applicationId}/Sequences/{sequenceNumber}/Sections/{sectionNumber}/Page")]
        public async Task<AssessorPage> GetFirstModeratorPage(Guid applicationId, int sequenceNumber, int sectionNumber)
        {
            return await GetModeratorPage(applicationId, sequenceNumber, sectionNumber, null);
        }

        [HttpGet("Moderator/Applications/{applicationId}/Sequences/{sequenceNumber}/Sections/{sectionNumber}/Page/{pageId}")]
        public async Task<AssessorPage> GetModeratorPage(Guid applicationId, int sequenceNumber, int sectionNumber, string pageId)
        {
            return await _pageService.GetPage(applicationId, sequenceNumber, sectionNumber, pageId);
        }

        [HttpGet("Moderator/Applications/{applicationId}/Sequences/{sequenceNumber}/Sections/{sectionNumber}/Page/{pageId}/BlindAssessmentOutcome")]
        public async Task<BlindAssessmentOutcome> GetBlindAssessmentOutcome(Guid applicationId, int sequenceNumber, int sectionNumber, string pageId)
        {
            var blindAssessmentOutcome = await _mediator.Send(new GetBlindAssessmentOutcomeRequest(applicationId, sequenceNumber, sectionNumber, pageId));

            return blindAssessmentOutcome;
        }

        [HttpPost("Moderator/Applications/{applicationId}/SubmitPageReviewOutcome")]
        public async Task SubmitPageReviewOutcome(Guid applicationId, [FromBody] SubmitPageReviewOutcomeCommand request)
        {
            await _mediator.Send(new SubmitModeratorPageOutcomeRequest(applicationId, request.SequenceNumber, request.SectionNumber, request.PageId, request.UserId, request.UserName, request.Status, request.Comment));
        }

        [HttpPost("Moderator/Applications/{applicationId}/GetPageReviewOutcome")]
        public async Task<ModeratorPageReviewOutcome> GetPageReviewOutcome(Guid applicationId, [FromBody] GetPageReviewOutcomeRequest request)
        {
            var pageReviewOutcome = await _mediator.Send(new GetModeratorPageReviewOutcomeRequest(applicationId, request.SequenceNumber, request.SectionNumber, request.PageId, request.UserId));

            return pageReviewOutcome;
        }

        [HttpPost("Moderator/Applications/{applicationId}/GetPageReviewOutcomesForSection")]
        public async Task<List<ModeratorPageReviewOutcome>> GetPageReviewOutcomesForSection(Guid applicationId, [FromBody] GetPageReviewOutcomesForSectionRequest request)
        {
            var moderatorReviewOutcomes = await _mediator.Send(new GetModeratorPageReviewOutcomesForSectionRequest(applicationId, request.SequenceNumber, request.SectionNumber, request.UserId));

            return moderatorReviewOutcomes;
        }

        [HttpPost("Moderator/Applications/{applicationId}/GetAllPageReviewOutcomes")]
        public async Task<List<ModeratorPageReviewOutcome>> GetAllPageReviewOutcomes(Guid applicationId, [FromBody] GetAllPageReviewOutcomesRequest request)
        {
            var moderatorReviewOutcomes = await _mediator.Send(new GetAllModeratorPageReviewOutcomesRequest(applicationId, request.UserId));

            return moderatorReviewOutcomes;
        }


        [HttpPost("Moderator/Applications/{applicationId}/SubmitOutcome")]
        public async Task SubmitPageReviewOutcome(Guid applicationId, [FromBody] SubmitModeratorOutcomeCommand request)
        {
            await _mediator.Send(new SubmitModeratorOutcomeRequest(applicationId, request.UserId, request.UserName, request.Status, request.Comment));
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
        }

        public class SubmitModeratorOutcomeCommand
        {
            public string UserId { get; set; }
            public string UserName { get; set; }
            public string Status { get; set; }
            public string Comment { get; set; }
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
