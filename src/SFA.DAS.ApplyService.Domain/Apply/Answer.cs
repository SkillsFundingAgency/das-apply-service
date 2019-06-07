using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Text.RegularExpressions;

namespace SFA.DAS.ApplyService.Domain.Apply
{
    public class Answer
    {
        public string QuestionId { get; set; }
        
        [JsonIgnore]
        public string Value
        {
            get { return JsonValue as string; }
            set { JsonValue = value; }
        }

        [JsonProperty(PropertyName = "Value")]
        public dynamic JsonValue { get; set; }

        public bool HasValue(Regex inputEntered)
        {
            if (JsonValue == null)
                return false;

            if (JsonValue is string stringValue)
            {
                return inputEntered.IsMatch(stringValue);
            }

            var jsonValue = new JObject(JsonValue);
            return jsonValue.Properties().All(p => inputEntered.IsMatch(p.Value.ToString()));
        }

        public bool HasSameValue(Answer other)
        {
            if (other != null)
            {
                if (JsonValue is string stringValue && other.JsonValue is string otherStringValue)
                {
                    return otherStringValue.Equals(stringValue);
                }

                if (JsonValue != null && other.JsonValue != null)
                {
                    var jsonValue = new JObject(JsonValue);
                    var otherJsonValue = new JObject(other.JsonValue);
                    return JToken.DeepEquals(otherJsonValue, jsonValue);
                }
            }

            return false;
        }

        public override string ToString()
        {
            if (JsonValue == null)
                return null;

            if (JsonValue is string stringValue)
            {
                return stringValue;
            }

            var jsonValue = new JObject(JsonValue);
            return string.Join(",", jsonValue.Properties().Select(p => p.Value.ToString()));
        }
    }
}