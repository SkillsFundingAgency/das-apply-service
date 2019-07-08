using System.Collections.Generic;

namespace SFA.DAS.ApplyService.Domain.Ukrlp
{
    public class UkrlpLookupResults
    {
        public bool Success { get; set; }
        public List<ProviderDetails> Results { get; set; }
    }
}
