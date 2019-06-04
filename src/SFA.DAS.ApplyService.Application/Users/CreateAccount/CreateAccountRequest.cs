using MediatR;

namespace SFA.DAS.ApplyService.Application.Users.CreateAccount
{
    public class CreateAccountRequest : IRequest<bool>
    {
        public CreateAccountRequest(string email, string givenName, string familyName, bool fromAssessor = false)
        {
            Email = email;
            GivenName = givenName;
            FamilyName = familyName;
            FromAssessor = fromAssessor;
        }

        public string Email { get; set; }
        public string FamilyName { get; set; }
        public string GivenName { get; set; }
        public bool FromAssessor { get; set; }
    }
}