using MediatR;
using System;

namespace SFA.DAS.ApplyService.Application.Apply.Roatp
{
    public class GetApplicationSectionNextPageRequest : IRequest<ApplicationSectionPageResponse>
    {
        public Guid ApplicationId { get; set; }
        public int SequenceId { get; set; }
        public int SectionId { get; set; }
        public string PageId { get; set; }

        public GetApplicationSectionNextPageRequest()
        {

        }

        public GetApplicationSectionNextPageRequest(Guid applicationId, int sequenceId, int sectionId, string pageId)
        {
            ApplicationId = applicationId;
            SequenceId = sequenceId;
            SectionId = sectionId;
            PageId = pageId;
        }
    }
}
