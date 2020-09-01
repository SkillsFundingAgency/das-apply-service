using System;
using MediatR;

namespace SFA.DAS.ApplyService.Application.Apply.Assessor
{
    public class AssignAssessorRequest : IRequest
    {
        public AssignAssessorRequest(Guid applicationId, int assessorNumber, string assessorUserId, string assessorName)
        {
            ApplicationId = applicationId;
            AssessorNumber = assessorNumber;
            AssessorUserId = assessorUserId;
            AssessorName = assessorName;
        }

        public Guid ApplicationId { get; }
        public int AssessorNumber { get; }
        public string AssessorUserId { get; }
        public string AssessorName { get; }
    }
}
