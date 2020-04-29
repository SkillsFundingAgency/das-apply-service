using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Application.Apply.Roatp;
using SFA.DAS.ApplyService.InternalApi.Infrastructure;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.InternalApi.Services
{
    public class ApplicationSectionOrchestrator : IApplicationSectionOrchestrator
    {
        private readonly IInternalQnaApiClient _qnaApiClient;
        private readonly ILogger<ApplicationSectionOrchestrator> _logger;
        private const string EndOfSectionNextAction = "ReturnToSection";

        public ApplicationSectionOrchestrator(IInternalQnaApiClient qnaApiClient, ILogger<ApplicationSectionOrchestrator> logger)
        {
            _qnaApiClient = qnaApiClient;
            _logger = logger;
        }

        public async Task<ApplicationSectionPageResponse> GetFirstPage(GetApplicationSectionFirstPageRequest request)
        {
            var applicationSection = await _qnaApiClient.GetSectionBySectionNo(request.ApplicationId, request.SequenceId, request.SectionId);

            // QnA API will filter out any irrelevant pages

            var startPage = applicationSection.QnAData.Pages.FirstOrDefault();

            if (startPage != null)
            {
                var hasFurtherPages = startPage.Next.Any(x => x.Action != EndOfSectionNextAction);
                var response = new ApplicationSectionPageResponse
                {
                    Page = startPage,       // strip QnA page down to bare minimum required to render on screen
                    IsLastPage = !hasFurtherPages
                };
                return await Task.FromResult(response);
            }

            return await Task.FromResult(new ApplicationSectionPageResponse { Page = null, IsLastPage = true });
        }

        public async Task<ApplicationSectionPageResponse> GetNextPage(GetApplicationSectionNextPageRequest request)
        {
            var applicationSection = await _qnaApiClient.GetSectionBySectionNo(request.ApplicationId, request.SequenceId, request.SectionId);
            
            var nextPageAction = await _qnaApiClient.SkipPageBySectionNo(request.ApplicationId, request.SequenceId, request.SectionId, request.PageId);
            if (nextPageAction.NextAction != EndOfSectionNextAction)
            {
                var nextPage = await _qnaApiClient.GetPageBySectionNo(request.ApplicationId, request.SequenceId, request.SectionId, nextPageAction.NextActionId);
                var hasFurtherPages = nextPage.Next.Any(x => x.Action != EndOfSectionNextAction);
                var response = new ApplicationSectionPageResponse
                {
                    Page = nextPage,       // strip QnA page down to bare minimum required to render on screen
                    IsLastPage = !hasFurtherPages
                };
                return await Task.FromResult(response);
            }

            return await Task.FromResult(new ApplicationSectionPageResponse { Page = null, IsLastPage = true });
        }
    }
}
