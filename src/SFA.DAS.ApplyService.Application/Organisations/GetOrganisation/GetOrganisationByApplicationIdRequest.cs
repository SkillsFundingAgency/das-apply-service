using System;
using MediatR;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Application.Organisations.GetOrganisation
{
    public class GetOrganisationByApplicationIdRequest : IRequest<Organisation>
    {
        public GetOrganisationByApplicationIdRequest(Guid applicationId)
        {
            ApplicationId = applicationId;
        }

        public Guid ApplicationId { get; set; }
    }
}