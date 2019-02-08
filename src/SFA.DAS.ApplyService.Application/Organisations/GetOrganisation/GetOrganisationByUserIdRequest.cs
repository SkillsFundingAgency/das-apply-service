using System;

namespace SFA.DAS.ApplyService.Application.Organisations.GetOrganisation
{
    using MediatR;
    using SFA.DAS.ApplyService.Domain.Entities;

    public class GetOrganisationByUserIdRequest : IRequest<Organisation>
    {
        public Guid UserId { get; set; }
    }
}
