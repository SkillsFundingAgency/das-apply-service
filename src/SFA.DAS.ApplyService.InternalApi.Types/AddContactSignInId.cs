using System;
using System.Collections.Generic;
using System.Text;

namespace SFA.DAS.ApplyService.InternalApi.Types
{
    public class AddContactSignInId
    {
        public string Email { get; set; }
        public string SignInId { get; set; }
        public string ContactId { get; set; }
        public string UpdatedBy { get; set; }
    }
}
