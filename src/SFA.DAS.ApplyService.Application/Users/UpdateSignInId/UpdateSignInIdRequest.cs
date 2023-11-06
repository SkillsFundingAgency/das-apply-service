using System;
using MediatR;

namespace SFA.DAS.ApplyService.Application.Users.UpdateSignInId
{
    public class UpdateSignInIdRequest : IRequest
    {
        public Guid SignInId { get; }
        public Guid ContactId { get; }
        public string GovUkIdentifier { get; }

        public UpdateSignInIdRequest(Guid signInId, Guid contactId, string govUkIdentifier)
        {
            SignInId = signInId;
            ContactId = contactId;
            GovUkIdentifier = govUkIdentifier;
        }
    }
}