using System.Net.Http;

namespace SFA.DAS.ApplyService.Web.Infrastructure
{
    public class ApiClient
    {
        private readonly HttpClient _httpClient;

        public ApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        
        
    }
}