namespace SFA.DAS.ApplyService.Configuration
{
    public class GovUkOidcConfiguration : IGovUkOidcConfiguration
    {
        public string BaseUrl { get; set; }
        public string ClientId { get; set; }
        public string KeyVaultIdentifier { get; set; }
        public string GovLoginSessionConnectionString { get; set; }
        public string LoginSlidingExpiryTimeOutInMinutes { get; set; }
    }
}
