using System.Net.Http;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Web.ViewModels;

namespace SFA.DAS.ApplyService.Web.Infrastructure
{
    public class UsersApiClient 
    {
        private readonly HttpClient _httpClient;

        public UsersApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> InviteUser(CreateAccountViewModel vm)
        {
            var result = await _httpClient.PostAsJsonAsync("/Account/", vm);
            return result.IsSuccessStatusCode;
        }

        public async Task<Contact> GetUserBySignInId(string signInId)
        {
            return await (await _httpClient.GetAsync($"/Account/{signInId}")).Content.ReadAsAsync<Contact>();
        }
    }
}