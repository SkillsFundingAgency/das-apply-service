using MediatR;
using SFA.DAS.ApplyService.Domain.Entities;
using System;

namespace SFA.DAS.ApplyService.Application.Organisations.GetOrganisation
{
    public class GetOrganisationByIdRequest : IRequest<Organisation>
    {
        public GetOrganisationByIdRequest(Guid organisationId)
        {
            OrganisationId = organisationId;
        }

        public Guid OrganisationId { get; set; }
    }
}
