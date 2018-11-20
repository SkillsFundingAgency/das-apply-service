using System.Collections.Generic;

namespace SFA.DAS.ApplyService.InternalApi.Models.ProviderRegister
{
    public class Provider
    {
        public int? Ukprn { get; set; }
        public bool IsHigherEducationInstitute { get; set; }
        public bool IsEmployerProvider { get; set; }
        public string ProviderName { get; set; }
        public IEnumerable<string> Aliases { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public IEnumerable<Address> Addresses { get; set; }
    }
}
