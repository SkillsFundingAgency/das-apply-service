using System.Runtime.InteropServices;

namespace SFA.DAS.ApplyService.Configuration
{
    public interface IApplyConfig
    {
        InternalApiConfig InternalApi { get; set; }
        string SignInPage { get; set; }
        string SessionRedisConnectionString { get; set; }
        DfeSignInConfig DfeSignIn { get; set; }
        string SqlConnectionString { get; set; }
        FileStorageConfig FileStorage { get; set; }
        
        NotificationsApiClientConfiguration NotificationsApiClientConfiguration { get; set; }

        AssessorServiceApiAuthentication AssessorServiceApiAuthentication { get; set; }
        ProviderRegisterApiAuthentication ProviderRegisterApiAuthentication { get; set; }
        ReferenceDataApiAuthentication ReferenceDataApiAuthentication { get; set; }

        CompaniesHouseApiAuthentication CompaniesHouseApiAuthentication { get; set; }

        string FeedbackUrl { get; set; }
    }
}