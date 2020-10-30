namespace SFA.DAS.ApplyService.Web.ViewModels
{
    public class CreateAccountViewModel
    {
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        public string Email { get; set; }

        public override string ToString()
        {
            return $"{GivenName} {FamilyName}, {Email}";
        }
    }
}