﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SFA.DAS.ApplyService.Application.Apply.Assessor
{
    public class SubmitAssessorPageOutcomeHandlerRequest : IRequest
    {
        public SubmitAssessorPageOutcomeHandlerRequest(Guid applicationId,
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

        public Guid ApplicationId { get; set; }
        public int SequenceNumber { get; set; }
        public int SectionNumber { get; set; }
        public string PageId { get; set; }
        public int AssessorType { get; set; }
        public string UserId { get; set; }
        public string Status { get; set; }
        public string Comment { get; set; }
    }
}