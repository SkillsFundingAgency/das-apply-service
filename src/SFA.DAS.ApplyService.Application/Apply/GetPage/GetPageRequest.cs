using System;
using MediatR;
using SFA.DAS.ApplyService.Domain.Apply;

namespace SFA.DAS.ApplyService.Application.Apply.GetPage
{
    public class GetPageRequest : IRequest<Page>
    {
        public Guid ApplicationId { get; }
        public string PageId { get; }
        public Guid UserId { get; }

        public GetPageRequest(Guid applicationId, string pageId, Guid userId)
        {
            ApplicationId = applicationId;
            PageId = pageId;
            UserId = userId;
        }
    }
}