using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Application.Apply.Clarification;
using SFA.DAS.ApplyService.Domain.Apply.Clarification;
using SFA.DAS.ApplyService.Infrastructure.ApiClients;
using SFA.DAS.ApplyService.InternalApi.Types.Assessor;
using SFA.DAS.ApplyService.InternalApi.Types.Responses.Oversight;

namespace SFA.DAS.ApplyService.Web.Infrastructure
{
    public class OutcomeApiClient : ApiClientBase<OutcomeApiClient>, IOutcomeApiClient
    {
        public OutcomeApiClient(HttpClient httpClient, ILogger<OutcomeApiClient> logger, ITokenService tokenService) : base(httpClient, logger)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenService.GetToken());

        }

        public async Task<GetOversightReviewResponse> GetOversightReview(Guid applicationId)
        {
            return await Get<GetOversightReviewResponse>($"/Oversight/{applicationId}/review");
        }

        public async Task<List<AssessorSequence>> GetClarificationSequences(Guid applicationId)
        {
            return await Get<List<AssessorSequence>>($"/Clarification/Applications/{applicationId}/Overview");
        }

        public async Task<List<ClarificationPageReviewOutcome>> GetAllClarificationPageReviewOutcomes(Guid applicationId, string userId)
        {
            return await Post<GetAllClarificationPageReviewOutcomesRequest, List<ClarificationPageReviewOutcome>>($"/Clarification/Applications/{applicationId}/GetAllPageReviewOutcomes", new GetAllClarificationPageReviewOutcomesRequest(applicationId, userId));
        }

        public async Task<AssessorPage> GetAssessorPage(Guid applicationId, int sequenceNumber, int sectionNumber, string pageId)
        {
            AssessorPage assessorPage;

            if (string.IsNullOrEmpty(pageId))
            {
                assessorPage = await Get<AssessorPage>($"/Assessor/Applications/{applicationId}/Sequences/{sequenceNumber}/Sections/{sectionNumber}/Page");
            }
            else
            {
                assessorPage = await Get<AssessorPage>($"/Assessor/Applications/{applicationId}/Sequences/{sequenceNumber}/Sections/{sectionNumber}/Page/{pageId}");
            }

            return assessorPage;
        }
    }
}
