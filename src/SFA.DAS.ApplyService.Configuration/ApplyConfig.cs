namespace SFA.DAS.ApplyService.Configuration
{
    public class ApplyConfig : IApplyConfig
    {
        public string SessionRedisConnectionString { get; set; }
        public DfeSignInConfig DfeSignIn { get; set; }
        public string SqlConnectionString { get; set; }
    }
}