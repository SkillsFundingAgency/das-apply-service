using System.Collections.Generic;

namespace SFA.DAS.ApplyService.InternalApi.Models.Ukrlp
{
    public class UkprnLookupResponse
    {
        public bool Success { get; set; }
        public List<ProviderDetails> Results { get; set; }
    }
}
