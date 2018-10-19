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
        EmailConfig Email { get; set; }
    }

    public class InternalApiConfig
    {
        public string Uri { get; set; }
    }
}