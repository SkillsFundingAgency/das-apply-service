using Newtonsoft.Json;

namespace SFA.DAS.ApplyService.Configuration
{
    public class InternalApiConfig
    {
        public string Uri { get; set; }
        [JsonRequired] public string Instance { get; set; }

        [JsonRequired] public string TenantId { get; set; }

        [JsonRequired] public string ClientId { get; set; }

        [JsonRequired] public string ClientSecret { get; set; }

        [JsonRequired] public string ResourceId { get; set; }
    }
}