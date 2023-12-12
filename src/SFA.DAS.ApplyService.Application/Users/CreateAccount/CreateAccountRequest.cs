using MediatR;

namespace SFA.DAS.ApplyService.Application.Users.CreateAccount
{
    public class CreateAccountRequest : IRequest<bool>
    {
        public CreateAccountRequest(string email, string givenName, string familyName, string govUkIdentifier)
        {
            Email = email;
            GivenName = givenName;
            FamilyName = familyName;
            GovUkIdentifier = govUkIdentifier;
        }

        public string GovUkIdentifier { get; set; }

        public string Email { get; set; }
        public string FamilyName { get; set; }
        public string GivenName { get; set; }
    }
}