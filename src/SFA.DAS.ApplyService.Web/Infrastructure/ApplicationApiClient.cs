using System.Net.Http;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Web.ViewModels;

namespace SFA.DAS.ApplyService.Web.Infrastructure
{
    public class ApplicationApiClient 
    {
        private readonly HttpClient _httpClient;

        public ApplicationApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

//        public async Task<bool> InviteUser(CreateAccountViewModel vm)
//        {
//            var result = await _httpClient.PostAsJsonAsync("/Account/", vm);
//            return result.IsSuccessStatusCode;
//        }

//        public async Task<Sequence> GetSequence(string signInId)
//        {
//            return await (await _httpClient.GetAsync($"/Account/{signInId}")).Content.ReadAsAsync<Contact>();
//        }
    }
}