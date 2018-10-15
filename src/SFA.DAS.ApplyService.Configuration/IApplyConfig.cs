namespace SFA.DAS.ApplyService.Configuration
{
    public interface IApplyConfig
    {
        string SessionRedisConnectionString { get; set; }
        DfeSignInConfig DfeSignIn { get; set; }
        string SqlConnectionString { get; set; }
    }
}