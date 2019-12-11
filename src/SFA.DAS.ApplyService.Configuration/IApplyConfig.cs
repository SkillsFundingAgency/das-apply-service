using System.Runtime.InteropServices;
using Newtonsoft.Json;

namespace SFA.DAS.ApplyService.Configuration
{
    public interface IApplyConfig
    {
        ApiAuthentication ApiAuthentication { get; set; }
        InternalApiConfig InternalApi { get; set; }
        string SignInPage { get; set; }
        string SessionRedisConnectionString { get; set; }
        DfeSignInConfig DfeSignIn { get; set; }
        string SqlConnectionString { get; set; }

        
        NotificationsApiClientConfiguration NotificationsApiClientConfiguration { get; set; }

        AssessorServiceApiAuthentication AssessorServiceApiAuthentication { get; set; }
        ProviderRegisterApiAuthentication ProviderRegisterApiAuthentication { get; set; }
        ReferenceDataApiAuthentication ReferenceDataApiAuthentication { get; set; }

        CompaniesHouseApiAuthentication CompaniesHouseApiAuthentication { get; set; }
        CharityCommissionApiAuthentication CharityCommissionApiAuthentication { get; set; }

        string FeedbackUrl { get; set; }
        string AssessorServiceBaseUrl { get; set; }

        RoatpApiAuthentication RoatpApiAuthentication { get; set; }

        UkrlpApiAuthentication UkrlpApiAuthentication { get; set; }

        QnaApiAuthentication QnaApiAuthentication { get; set; }
    }

    public class ApiAuthentication
    {
        [JsonRequired] public string ClientId { get; set; }

        [JsonRequired] public string Instance { get; set; }

        [JsonRequired] public string TenantId { get; set; }

        [JsonRequired] public string Audience { get; set; }
    }
}