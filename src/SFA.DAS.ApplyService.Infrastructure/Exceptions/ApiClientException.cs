using System;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;

namespace SFA.DAS.ApplyService.Infrastructure.Exceptions
{
    [Serializable]
    public class ApiClientException : ApplicationException
    {
        public string HttpMethod { get; }
        public HttpStatusCode StatusCode { get; }
        public Uri RequestUri { get; }

        public ApiClientException()
        {
        }

        public ApiClientException(HttpResponseMessage httpResponseMessage, string message)
            : this(httpResponseMessage?.RequestMessage?.Method.ToString(),
                  httpResponseMessage?.StatusCode ?? HttpStatusCode.BadRequest,
                  httpResponseMessage?.RequestMessage?.RequestUri,
                  message)
        {
        }

        public ApiClientException(HttpResponseMessage httpResponseMessage, string message, Exception innerException)
            : this(httpResponseMessage?.RequestMessage?.Method.ToString(),
                  httpResponseMessage?.StatusCode ?? HttpStatusCode.BadRequest,
                  httpResponseMessage?.RequestMessage?.RequestUri,
                  message,
                  innerException)
        {
        }

        public ApiClientException(string httpMethod, HttpStatusCode statusCode, Uri requestUri, string message) : base(message)
        {
            HttpMethod = httpMethod;
            StatusCode = statusCode;
            RequestUri = requestUri;
        }

        public ApiClientException(string httpMethod, HttpStatusCode statusCode, Uri requestUri, string message, Exception innerException) : base(message, innerException)
        {
            HttpMethod = httpMethod;
            StatusCode = statusCode;
            RequestUri = requestUri;
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        protected ApiClientException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            HttpMethod = info.GetValue("HttpMethod", typeof(string)) as string;
            StatusCode = (HttpStatusCode)info.GetValue("StatusCode", typeof(HttpStatusCode));
            RequestUri = info.GetValue("RequestUri", typeof(Uri)) as Uri;
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }
            info.AddValue("HttpMethod", HttpMethod);
            info.AddValue("StatusCode", StatusCode);
            info.AddValue("RequestUri", RequestUri);
            base.GetObjectData(info, context);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"HTTP {(int)StatusCode} || {HttpMethod}: {RequestUri}");

            if (!string.IsNullOrWhiteSpace(Message))
            {
                sb.AppendLine();
                sb.Append($"Message: {Message}");
            }

            if (InnerException != null)
            {
                sb.AppendLine();
                sb.Append($"InnerException: {InnerException.Message}");
            }

            return sb.ToString();
        }
    }
}