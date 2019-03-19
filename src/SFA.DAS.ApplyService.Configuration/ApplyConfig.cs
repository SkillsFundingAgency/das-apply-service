namespace SFA.DAS.ApplyService.Configuration
{
    public class ApplyConfig : IApplyConfig
    {
        public InternalApiConfig InternalApi { get; set; }
        public string SignInPage { get; set; }
        public string SessionRedisConnectionString { get; set; }
        public DfeSignInConfig DfeSignIn { get; set; }
        public string SqlConnectionString { get; set; }
        public FileStorageConfig FileStorage { get; set; }

        public NotificationsApiClientConfiguration NotificationsApiClientConfiguration { get; set; }
        public AssessorServiceApiAuthentication AssessorServiceApiAuthentication { get; set; }
        public ProviderRegisterApiAuthentication ProviderRegisterApiAuthentication { get; set; }
        public ReferenceDataApiAuthentication ReferenceDataApiAuthentication { get; set; }

        public CompaniesHouseApiAuthentication CompaniesHouseApiAuthentication { get; set; }
        public CharityCommissionApiAuthentication CharityCommissionApiAuthentication { get; set; }

        public string FeedbackUrl { get; set; }

        public string AssessorServiceBaseUrl { get; set; }
    }
}