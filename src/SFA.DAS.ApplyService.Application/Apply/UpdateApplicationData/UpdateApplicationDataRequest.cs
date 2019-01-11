using System;
using MediatR;

namespace SFA.DAS.ApplyService.Application.Apply.UpdateApplicationData
{
    public class UpdateApplicationDataRequest : IRequest
    {
        public Guid ApplicationId { get; }
        public object ApplicationData { get; }
        public UpdateApplicationDataRequest(Guid applicationId, object applicationData)
        {
            ApplicationId = applicationId;
            ApplicationData = applicationData;
        }
    }
}