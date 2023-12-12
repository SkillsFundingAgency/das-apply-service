using Newtonsoft.Json;

namespace SFA.DAS.ApplyService.Configuration
{
    public class ApplyConfig : IApplyConfig
    {
        [JsonRequired]
        public InternalApiConfig InternalApi { get; set; }
        public string SignInPage { get; set; }
        public string SessionRedisConnectionString { get; set; }
        public string SessionCachingDatabase { get; set; }
        public string DataProtectionKeysDatabase { get; set; }
        [JsonRequired]
        public DfeSignInConfig DfeSignIn { get; set; }
        public string SqlConnectionString { get; set; }
        [JsonRequired]
        public FileStorageConfig FileStorage { get; set; }
        [JsonRequired]
        public NotificationsApiClientConfiguration NotificationsApiClientConfiguration { get; set; }
        [JsonRequired]
        public CompaniesHouseApiAuthentication CompaniesHouseApiAuthentication { get; set; }
        [JsonRequired]
        public OuterApiConfiguration OuterApiConfiguration { get; set; }
        [JsonRequired]
        public RoatpApiAuthentication RoatpApiAuthentication { get; set; }
        [JsonRequired]
        public QnaApiAuthentication QnaApiAuthentication { get; set; }
        [JsonRequired]
        public AzureActiveDirectoryConfiguration AzureActiveDirectoryConfiguration { get; set; }
        [JsonRequired]
        public FeatureToggles FeatureToggles { get; set; }

        //<inherit-doc/>
        public bool UseGovSignIn { get; set; }
    }
}