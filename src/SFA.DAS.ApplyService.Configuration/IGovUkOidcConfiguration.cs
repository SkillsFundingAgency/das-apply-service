namespace SFA.DAS.ApplyService.Configuration
{
    public interface IGovUkOidcConfiguration
    {
        string BaseUrl { get; set; }
        string ClientId { get; set; }
        string KeyVaultIdentifier { get; set; }
        string GovLoginSessionConnectionString { get; set; }
        string LoginSlidingExpiryTimeOutInMinutes { get; set; }
    }
}
