using System;
using MediatR;

namespace SFA.DAS.ApplyService.Application.Apply.Review.Return
{
    public class ReturnRequest : IRequest
    {
        public Guid ApplicationId { get; }
        public int SequenceId { get; }
        public string RequestReturnType { get; }

        public ReturnRequest(Guid applicationId, int sequenceId, string requestReturnType)
        {
            ApplicationId = applicationId;
            SequenceId = sequenceId;
            RequestReturnType = requestReturnType;
        }
    }
}