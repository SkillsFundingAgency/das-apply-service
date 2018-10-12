using MediatR;

namespace SFA.DAS.ApplyService.Application.Users.CreateAccount
{
    public class CreateAccountRequest : IRequest
    {
        public CreateAccountRequest(string email, string givenName, string familyName)
        {
            Email = email;
            GivenName = givenName;
            FamilyName = familyName;
        }

        public string Email { get; set; }
        public string FamilyName { get; set; }
        public string GivenName { get; set; }
    }
}