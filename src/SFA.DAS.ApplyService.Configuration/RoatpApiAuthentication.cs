using Newtonsoft.Json;

namespace SFA.DAS.ApplyService.Configuration
{
    public class RoatpApiAuthentication
    {
        [JsonRequired] public string ApiBaseAddress { get; set; }
        [JsonRequired] public string Identifier { get; set; }
    }
}