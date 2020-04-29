using MediatR;
using System;

namespace SFA.DAS.ApplyService.Application.Apply.Roatp
{
    public class GetApplicationSectionFirstPageRequest : IRequest<ApplicationSectionPageResponse>
    {
        public Guid ApplicationId { get; set; }
        public int SequenceId { get; set; }
        public int SectionId { get; set; }
       
        public GetApplicationSectionFirstPageRequest()
        {

        }

        public GetApplicationSectionFirstPageRequest(Guid applicationId, int sequenceId, int sectionId)
        {
            ApplicationId = applicationId;
            SequenceId = sequenceId;
            SectionId = sectionId;
        }
    }
}
