using System;
using System.Collections.Generic;
using MediatR;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Application.Users.GetOrganisationContacts
{
    public class GetOrganisationContactsRequest : IRequest<List<Contact>>
    {
        public Guid Id { get; }

        public GetOrganisationContactsRequest(Guid id)
        {
            Id = id;
        }
    }
}
