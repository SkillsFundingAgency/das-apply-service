using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using SFA.DAS.ApplyService.Domain.Entities;

namespace SFA.DAS.ApplyService.Application.Users.UpdateContactOrgId
{
    public class MigrateContactOrganisationRequest:IRequest
    {
        public MigrateContactOrganisationRequest(Contact contact, Organisation organisation)
        {
            Contact = contact;
            Organisation = organisation;
        }

        public Contact Contact { get; }
        public Organisation Organisation { get; }
    }
}
