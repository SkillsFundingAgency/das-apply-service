using System;
using MediatR;

namespace SFA.DAS.ApplyService.Application.Users.CreateAccountFromAsLogin
{
    public class CreateAccountFromAsLoginRequest : IRequest<bool>
    {
        public Guid SignInId { get; }
        public string Email { get; }
        public string GivenName { get; }
        public string FamilyName { get; }
        public string GovUkIdentifier { get; set; }

        public CreateAccountFromAsLoginRequest(Guid signInId, string email, string givenName, string familyName, string govUkIdentifier)
        {
            SignInId = signInId;
            Email = email;
            GivenName = givenName;
            FamilyName = familyName;
            GovUkIdentifier = govUkIdentifier;
        }
    }
}