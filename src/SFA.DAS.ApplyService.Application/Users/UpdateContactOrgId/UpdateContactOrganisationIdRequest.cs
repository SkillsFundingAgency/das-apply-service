using System;
using MediatR;

namespace SFA.DAS.ApplyService.Application.Users.UpdateContactOrgId
{
    public class UpdateContactOrganisationIdRequest : IRequest
    {
        public Guid ContactId { get; }
        public Guid OrgId { get; }

        public UpdateContactOrganisationIdRequest(Guid contactId, Guid orgId)
        {
            OrgId = orgId;
            ContactId = contactId;
        }
    }
}