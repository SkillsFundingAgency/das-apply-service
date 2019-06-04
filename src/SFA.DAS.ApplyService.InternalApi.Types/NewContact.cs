using System.Diagnostics;

namespace SFA.DAS.ApplyService.InternalApi.Types
{
    public class NewContact
    {
        public string Email { get; set; }    
        public string FamilyName { get; set; }    
        public string GivenName { get; set; }    

        public bool FromAssessor { get; set; }
    }
}