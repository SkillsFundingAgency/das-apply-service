using System;

namespace SFA.DAS.ApplyService.Domain.Apply.AllowedProviders
{
    public class AllowedProvider
    {
        public int Ukprn { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public DateTime? AddedDateTime { get; set; }
    }
}
