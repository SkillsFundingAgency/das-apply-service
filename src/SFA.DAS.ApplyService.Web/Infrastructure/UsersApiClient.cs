using System;
using System.Net.Http;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Configuration;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.InternalApi.Types;
using SFA.DAS.ApplyService.Web.ViewModels;

namespace SFA.DAS.ApplyService.Web.Infrastructure
{
    public interface IUsersApiClient
    {
        Task<bool> InviteUser(CreateAccountViewModel vm);

        Task<Contact> GetUserBySignInId(string signInId);
        Task Callback(DfeSignInCallback callback);
    }

    public class UsersApiClient : IUsersApiClient
    {
        private static readonly HttpClient HttpClient = new HttpClient();

        public UsersApiClient(IConfigurationService configService)
        {
            if (HttpClient.BaseAddress == null)
            {
                HttpClient.BaseAddress = new Uri(configService.GetConfig().Result.InternalApi.Uri);
            }
        }

        public async Task<bool> InviteUser(CreateAccountViewModel vm)
        {
            var result = await HttpClient.PostAsJsonAsync("/Account/", vm);
            return result.IsSuccessStatusCode;
        }

        public async Task<Contact> GetUserBySignInId(string signInId)
        {
            return await (await HttpClient.GetAsync($"/Account/{signInId}")).Content.ReadAsAsync<Contact>();
        }

        public async Task Callback(DfeSignInCallback callback)
        {
            await HttpClient.PostAsJsonAsync($"/Account/Callback", callback);
        }
    }
}