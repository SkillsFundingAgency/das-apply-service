namespace SFA.DAS.ApplyService.Configuration
{
    public class AssessorServiceApiAuthentication
    {
        public string TenantId { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; } // AppKey
        public string ResourceId { get; set; }

        public string ApiBaseAddress { get; set; }
    }
}
