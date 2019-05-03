using Newtonsoft.Json;

namespace SFA.DAS.ApplyService.Configuration
{
    public class RoatpApiAuthentication : IClientApiAuthentication
    {
        [JsonRequired] public string Instance { get; set; }

        [JsonRequired] public string TenantId { get; set; }

        [JsonRequired] public string ClientId { get; set; }

        [JsonRequired] public string ClientSecret { get; set; }

        [JsonRequired] public string ResourceId { get; set; }

        [JsonRequired] public string ApiBaseAddress { get; set; }
    }
}