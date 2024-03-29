﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Application.Apply.Clarification;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.Domain.Apply.Clarification;
using SFA.DAS.ApplyService.Infrastructure.ApiClients;
using SFA.DAS.ApplyService.InternalApi.Types.Assessor;
using SFA.DAS.ApplyService.InternalApi.Types.Responses.Oversight;
using SFA.DAS.ApplyService.Web.ViewModels.Roatp;

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

        public async Task<SectorDetails> GetClarificationSectorDetails(Guid applicationId, string pageId)
        {
            return await Get<SectorDetails>($"/Clarification/Applications/{applicationId}/SectorDetails/{pageId}");
        }

        public async  Task<HttpResponseMessage> DownloadClarificationfile(Guid applicationId, int sequenceNumber, int sectionNumber, string pageId,
            string filename)
        {
            return await GetResponse($"/Clarification/Applications/{applicationId}/Sequences/{sequenceNumber}/Sections/{sectionNumber}/Page/{pageId}/Download/{filename}");
        }

        public async Task<DateTime?> GetWorkingDaysAheadDate(DateTime? startDate, int numberOfDays)
        {
            if (startDate == null)
                return null;

            var startDateFormatted = startDate.Value.ToString("yyyy-MM-dd");
            return await Get<DateTime>($"working-days/{startDateFormatted}/{numberOfDays}");
        }

        public async Task<bool> ReapplicationRequested(Guid applicationId, string userId)
        {
            return await Post<ReapplicationRequest, bool>($"/Application/{applicationId}/ReapplicationRequested", new ReapplicationRequest { ApplicationId = applicationId, UserId = userId });
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
