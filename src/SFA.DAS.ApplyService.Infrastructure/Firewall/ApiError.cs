using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.ApplyService.Infrastructure.Firewall
{
    /// <summary>
    /// This is raised when the WAF (Web Application Firewall) intercepts what it thinks is a suspicious request
    /// or there is an internal network issue.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ApiError
    {
        public int StatusCode { get; private set; }

        public string StatusDescription { get; private set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Message { get; private set; }

        public ApiError()
        {
        }

        public ApiError(int statusCode, string statusDescription)
        {
            StatusCode = statusCode;
            StatusDescription = statusDescription;
        }

        public ApiError(int statusCode, string statusDescription, string message)
            : this(statusCode, statusDescription)
        {
            Message = message;
        }
    }
}
