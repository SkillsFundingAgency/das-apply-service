using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SFA.DAS.ApplyService.InternalApi.Infrastructure.CharityCommission.Entities
{
    public enum ClassificationType
    {
        What,
        Who,
        How
    }

    public class Classification
    {
        [JsonProperty("classification_code")]
        public string Code { get; set; }

        [JsonProperty("classification_type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public ClassificationType Type { get; set; }

        [JsonProperty("classification_desc")]
        public string Description { get; set; }
    }
}
