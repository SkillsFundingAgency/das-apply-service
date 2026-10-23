namespace SFA.DAS.ApplyService.Configuration
{
    public interface IApplyConfig
    {
        InternalApiConfig InternalApi { get; set; }
        string SignInPage { get; set; }
        string SessionRedisConnectionString { get; set; }
        string SessionCachingDatabase { get; set; }
        string DataProtectionKeysDatabase { get; set; }
        DfeSignInConfig DfeSignIn { get; set; }
        string SqlConnectionString { get; set; }

        FileStorageConfig FileStorage { get; set; }

        NotificationsApiClientConfiguration NotificationsApiClientConfiguration { get; set; }

        CompaniesHouseApiAuthentication CompaniesHouseApiAuthentication { get; set; }

        OuterApiConfiguration OuterApiConfiguration { get; set; }

        RoatpApiAuthentication RoatpApiAuthentication { get; set; }

        QnaApiAuthentication QnaApiAuthentication { get; set; }

        AzureActiveDirectoryConfiguration AzureActiveDirectoryConfiguration { get; set; }

        FeatureToggles FeatureToggles { get; set; }

        string ProvidersExemptedFromHavingTrustees { get; set; }

        /// <summary>
        /// Gets or Sets the UseGovSignIn value.
        /// </summary>
        bool UseGovSignIn { get; set; }

        /// <summary>
        /// Base URL of the CDN that serves shared front-end assets (CSS, images, fonts).
        /// </summary>
        string CdnBaseUrl { get; set; }
    }
}