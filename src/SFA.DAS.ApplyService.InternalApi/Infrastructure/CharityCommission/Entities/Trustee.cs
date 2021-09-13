using Newtonsoft.Json;

namespace SFA.DAS.ApplyService.InternalApi.Infrastructure.CharityCommission.Entities
{
    public class Trustee
    {
        [JsonProperty("organisation_number")]
        public long OrganisationNumber { get; set; }

        [JsonProperty("trustee_name")]
        public string Name { get; set; }
    }
}
