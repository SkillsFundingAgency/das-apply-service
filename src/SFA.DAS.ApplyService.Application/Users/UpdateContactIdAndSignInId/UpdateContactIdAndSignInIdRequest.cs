using System;
using MediatR;

namespace SFA.DAS.ApplyService.Application.Users.UpdateContactIdAndSignInId
{
    public class UpdateContactIdAndSignInIdRequest : IRequest
    {
        public Guid SignInId { get; }
        public Guid ContactId { get; }
        public string Email { get; }

        public string UpdatedBy { get; }
        public UpdateContactIdAndSignInIdRequest(Guid signInId, Guid contactId, string email, string updatedBy)
        {
            SignInId = signInId;
            ContactId = contactId;
            Email = email;
            UpdatedBy = updatedBy;
        }
    }
}