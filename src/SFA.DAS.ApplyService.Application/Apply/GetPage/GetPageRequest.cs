using System;
using MediatR;
using SFA.DAS.ApplyService.Domain.Apply;

namespace SFA.DAS.ApplyService.Application.Apply.GetPage
{
    public class GetPageRequest : IRequest<Page>
    {
        public Guid ApplicationId { get; }
        public int SequenceId { get; }
        public int SectionId { get; }
        public string PageId { get; }
        public Guid UserId { get; }

        public GetPageRequest(Guid applicationId, int sequenceId, int sectionId, string pageId, Guid userId)
        {
            ApplicationId = applicationId;
            SequenceId = sequenceId;
            SectionId = sectionId;
            PageId = pageId;
            UserId = userId;
        }
    }
}