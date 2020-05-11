using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SFA.DAS.ApplyService.Application.Apply.Assessor
{
    public class SubmitAssessorPageOutcomeRequest : IRequest
    {
        public SubmitAssessorPageOutcomeRequest(Guid applicationId,
                                                        int sequenceNumber,
                                                        int sectionNumber,
                                                        string pageId,
                                                        int assessorType,
                                                        string userId,
                                                        string status,
                                                        string comment)
        {
            ApplicationId = applicationId;
            SequenceNumber = sequenceNumber;
            SectionNumber = sectionNumber;
            PageId = pageId;
            AssessorType = assessorType;
            UserId = userId;
            Status = status;
            Comment = comment;
        }

        public Guid ApplicationId { get; }
        public int SequenceNumber { get; }
        public int SectionNumber { get; }
        public string PageId { get; }
        public int AssessorType { get; }
        public string UserId { get; }
        public string Status { get; }
        public string Comment { get; }
    }
}
