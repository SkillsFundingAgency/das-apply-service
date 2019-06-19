namespace SFA.DAS.ApplyService.Domain.Roatp
{
    using SFA.DAS.ApplyService.Domain.Ukrlp;

    public class ApplicationDetails
    {
        public int ApplicationRouteId { get; set; }
        public ProviderDetails UkrlpLookupDetails { get; set; }
        public long UKPRN { get; set; }
    }
}
