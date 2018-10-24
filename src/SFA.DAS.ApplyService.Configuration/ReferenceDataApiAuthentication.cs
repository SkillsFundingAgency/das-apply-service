namespace SFA.DAS.ApplyService.Configuration
{
    public class ReferenceDataApiAuthentication
    {
        public string TenantId { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; } // AppKey
        public string ResourceId { get; set; }

        public string ApiBaseAddress { get; set; }
    }
}
