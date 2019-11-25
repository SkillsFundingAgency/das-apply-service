using MediatR;
using SFA.DAS.ApplyService.Domain.Review;
using System;
using System.Collections.Generic;

namespace SFA.DAS.ApplyService.Application.Review.GetRejectedOutcomes
{
    public class GetRejectedOutcomesRequest : IRequest<List<Outcome>>
    {
        public Guid ApplicationId { get; }
        public string SectionId { get; }
        public string PageId { get; }

        public GetRejectedOutcomesRequest(Guid applicationId, string sectionId, string pageId)
        {
            ApplicationId = applicationId;
            SectionId = sectionId;
            PageId = pageId;
        }
    }
}
