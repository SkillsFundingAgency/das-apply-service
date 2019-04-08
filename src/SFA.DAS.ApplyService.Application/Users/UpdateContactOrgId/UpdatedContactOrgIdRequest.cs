using System;
using MediatR;

namespace SFA.DAS.ApplyService.Application.Users.UpdateContactOrgId
{
    public class UpdateContactOrgdRequest : IRequest
    {
     
        public Guid ContactId { get; }
        public Guid OrgId { get; }
        public UpdateContactOrgdRequest( Guid contactId, Guid orgId)
        {
            OrgId = orgId;
            ContactId = contactId;
        }
    }
}