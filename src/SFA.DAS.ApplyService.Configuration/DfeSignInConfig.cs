namespace SFA.DAS.ApplyService.Configuration
{
    public class DfeSignInConfig
    {
        public string MetadataAddress { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string ApiClientSecret { get; set; }
        public string ApiUri { get; set; }
        public string RedirectUri { get; set; }
        public string CallbackUri { get; set; }
        public string SignOutRedirectUri { get; set; }
    }
}