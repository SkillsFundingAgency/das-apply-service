using System;
using MediatR;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Application.Apply.GetOrganisationForApplication
{
    public class GetOrganisationForApplicationRequest : IRequest<Organisation>
    {
        public Guid ApplicationId { get; }

        public GetOrganisationForApplicationRequest(Guid applicationId)
        {
            ApplicationId = applicationId;
        }
    }
}