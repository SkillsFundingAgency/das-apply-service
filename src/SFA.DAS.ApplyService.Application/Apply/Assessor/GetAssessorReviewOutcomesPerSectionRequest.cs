﻿using MediatR;
using SFA.DAS.ApplyService.Domain.Apply;
using System;
using System.Collections.Generic;
using System.Text;

namespace SFA.DAS.ApplyService.Application.Apply.Assessor
{
    public class GetAssessorReviewOutcomesPerSectionRequest : IRequest<List<PageReviewOutcome>>
    {
        public GetAssessorReviewOutcomesPerSectionRequest(Guid applicationId,
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

        public Guid ApplicationId { get; }
        public int SequenceNumber { get; }
        public int SectionNumber { get; }
        public int AssessorType { get;  }
        public string UserId { get; }
    }
}
