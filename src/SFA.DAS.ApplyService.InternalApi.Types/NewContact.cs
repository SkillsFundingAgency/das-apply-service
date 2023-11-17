using System;

namespace SFA.DAS.ApplyService.InternalApi.Types
{
    public class NewContact
    {
        public string Email { get; set; }    
        public string FamilyName { get; set; }    
        public string GivenName { get; set; }
        public string GovUkIdentifier { get; set; }
        public Guid? UserId { get; set; }
    }
}