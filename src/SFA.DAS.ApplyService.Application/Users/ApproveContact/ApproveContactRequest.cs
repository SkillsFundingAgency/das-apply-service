using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SFA.DAS.ApplyService.Application.Users.ApproveContact
{
    public class ApproveContactRequest : IRequest<bool>
    {
        public ApproveContactRequest(Guid contactId)
        {
            ContactId = contactId;
        }

        public Guid ContactId { get;}
    }
}
