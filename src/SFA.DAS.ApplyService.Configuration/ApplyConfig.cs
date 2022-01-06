namespace SFA.DAS.ApplyService.Configuration
{
    public class ApplyConfig : IApplyConfig
    {
        public InternalApiConfig InternalApi { get; set; }
        public string SignInPage { get; set; }
        public string SessionRedisConnectionString { get; set; }
        public string SessionCachingDatabase { get; set; }
        public string DataProtectionKeysDatabase { get; set; }
        public DfeSignInConfig DfeSignIn { get; set; }
        public string SqlConnectionString { get; set; }

        public FileStorageConfig FileStorage { get; set; }

        public NotificationsApiClientConfiguration NotificationsApiClientConfiguration { get; set; }

        public CompaniesHouseApiAuthentication CompaniesHouseApiAuthentication { get; set; }

        public OuterApiConfiguration OuterApiConfiguration { get; set; }

        public RoatpApiAuthentication RoatpApiAuthentication { get; set; }

        public QnaApiAuthentication QnaApiAuthentication { get; set; }

        public AzureActiveDirectoryConfiguration AzureActiveDirectoryConfiguration { get; set; }

        public FeatureToggles FeatureToggles { get; set; }
    }
}