using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Domain.Apply.Clarification;
using SFA.DAS.ApplyService.InternalApi.Types.Assessor;
using SFA.DAS.ApplyService.InternalApi.Types.Responses.Oversight;
using SFA.DAS.ApplyService.Web.ViewModels.Roatp;

namespace SFA.DAS.ApplyService.Web.Infrastructure
{
    public interface IOutcomeApiClient
    {
        Task<List<ClarificationPageReviewOutcome>> GetAllClarificationPageReviewOutcomes(Guid applicationId, string userId);

        Task<AssessorPage> GetAssessorPage(Guid applicationId, int sequenceNumber, int sectionNumber, string pageId);

        Task<GetOversightReviewResponse> GetOversightReview(Guid applicationId);
        Task<List<AssessorSequence>> GetClarificationSequences(Guid applicationId);
        Task<SectorDetails> GetClarificationSectorDetails(Guid applicationId, string pageId);
        Task<HttpResponseMessage> DownloadClarificationfile(Guid applicationId, int sequenceNumber, int sectionNumber,
            string pageId, string filename);

        Task<DateTime?> GetWorkingDaysAheadDate(DateTime? startDate, int numberOfDays);
        Task<bool> ReapplicationRequested(Guid applicationId, string userId);
    }

}