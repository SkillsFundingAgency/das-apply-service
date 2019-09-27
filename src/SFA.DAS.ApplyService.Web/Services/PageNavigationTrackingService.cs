using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApplyService.Session;
using SFA.DAS.ApplyService.Web.Infrastructure;

namespace SFA.DAS.ApplyService.Web.Services
{
    public class PageNavigationTrackingService : Controller, IPageNavigationTrackingService
    {
        private readonly ISessionService _sessionService;
        private readonly IQnaApiClient _qnaApiClient;

        private const string ApplicationHistoryKey = "Roatp_Application_History";

        public PageNavigationTrackingService(ISessionService sessionService, IQnaApiClient qnaApiClient)
        {
            _sessionService = sessionService;
            _qnaApiClient = qnaApiClient;
        }

        public void AddPageToNavigationStack(string pageId)
        {
            var applicationPageHistory = _sessionService.Get<Stack<string>>(ApplicationHistoryKey);
            if (applicationPageHistory == null)
            {
                applicationPageHistory = new Stack<string>();
            }
            applicationPageHistory.Push(pageId);

            _sessionService.Set(ApplicationHistoryKey, applicationPageHistory);
        }

        public async Task<string> GetBackNavigationPageId(Guid applicationId, int sequenceId, int sectionId, string pageId)
        {
            string previousPageId = string.Empty;

            var sequences = await _qnaApiClient.GetSequences(applicationId);
            var selectedSequence = sequences.Single(x => x.SequenceId == sequenceId);
            var sections = await _qnaApiClient.GetSections(applicationId, selectedSequence.Id);
            var currentSection = sections.Single(x => x.SectionId == sectionId);

            var firstPageInSection = currentSection.QnAData.Pages[0];
            if (pageId == firstPageInSection.PageId)
            {
                return await Task.FromResult<string>(null);
            }

            var applicationPageHistory = _sessionService.Get<Stack<string>>(ApplicationHistoryKey);
            if (applicationPageHistory == null)
            {
                return await Task.FromResult<string>(null);
            }

            while (applicationPageHistory.Count > 0)
            {
                previousPageId = applicationPageHistory.Pop();
                if (previousPageId != pageId)
                {
                    _sessionService.Set(ApplicationHistoryKey, applicationPageHistory);
                    break;
                }
            }
            if (previousPageId == pageId)
            {
                return await Task.FromResult<string>(null);
            }

            return await Task.FromResult(previousPageId);
        }
    }
}
