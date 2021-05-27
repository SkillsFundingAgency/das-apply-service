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

        public CreateAccountFromAsLoginRequest(Guid signInId, string email, string givenName, string familyName)
        {
            SignInId = signInId;
            Email = email;
            GivenName = givenName;
            FamilyName = familyName;
        }
    }
}