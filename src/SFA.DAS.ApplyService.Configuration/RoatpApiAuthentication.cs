using Newtonsoft.Json;
using SFA.DAS.AdminService.Common.Settings;

namespace SFA.DAS.ApplyService.Configuration
{
    public class RoatpApiAuthentication : IManagedIdentityApiAuthentication
    {
        [JsonRequired] public string ApiBaseAddress { get; set; }
        public string Identifier { get; set; }
    }
}