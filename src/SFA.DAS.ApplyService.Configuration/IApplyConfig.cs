namespace SFA.DAS.ApplyService.Configuration
{
    public interface IApplyConfig
    {
        string SignInPage { get; set; }
        string SessionRedisConnectionString { get; set; }
        DfeSignInConfig DfeSignIn { get; set; }
        string SqlConnectionString { get; set; }
        EmailConfig Email { get; set; }
    }

    public class EmailConfig
    {
        public string SendGridApiKey { get; set; }
    }
}