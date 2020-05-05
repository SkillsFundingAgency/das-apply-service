using MediatR;
using SFA.DAS.ApplyService.Domain.Apply;
using System;
using System.Collections.Generic;
using System.Text;

namespace SFA.DAS.ApplyService.Application.Apply.Assessor
{
    public class GetAssessorReviewOutcomesPerSectionHandlerRequest : IRequest<List<PageReviewOutcome>>
    {
        public GetAssessorReviewOutcomesPerSectionHandlerRequest(Guid applicationId,
                                                                    int sequenceNumber,
                                                                    int sectionNumber,
                                                                    int assessorType,
                                                                    string userId)
        {
            ApplicationId = applicationId;
            SequenceNumber = sequenceNumber;
            SectionNumber = sectionNumber;
            AssessorType = assessorType;
            UserId = userId;
        }

        public Guid ApplicationId { get; set; }
        public int SequenceNumber { get; set; }
        public int SectionNumber { get; set; }
        public int AssessorType { get; set; }
        public string UserId { get; set; }
    }
}
