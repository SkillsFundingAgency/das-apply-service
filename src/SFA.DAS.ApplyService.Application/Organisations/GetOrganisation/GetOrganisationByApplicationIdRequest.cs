using System;
using MediatR;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Application.Organisations.GetOrganisation
{
    public class GetOrganisationByApplicationIdRequest : IRequest<Organisation>
    {
        public Guid ApplicationId { get;  }
        public GetOrganisationByApplicationIdRequest(Guid applicationId)
        {
            ApplicationId = applicationId;
        }

        
    }
}