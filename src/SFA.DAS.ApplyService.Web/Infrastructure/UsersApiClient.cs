using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
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

        Task<bool> ApproveUser(Guid userId);
        Task Callback(DfeSignInCallback callback);
    }

    public class UsersApiClient : IUsersApiClient
    {
        private readonly ILogger<UsersApiClient> _logger;
        private static readonly HttpClient HttpClient = new HttpClient();

        public UsersApiClient(IConfigurationService configService, ILogger<UsersApiClient> logger)
        {
            _logger = logger;
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
            var httpResponseMessage = await HttpClient.GetAsync($"/Account/{signInId}");

            var contactJson = await httpResponseMessage.Content.ReadAsStringAsync();

            _logger.LogInformation($"GetUserBySignInId result: {contactJson}");
            
            return JsonConvert.DeserializeObject<Contact>(contactJson);

            //return await httpResponseMessage.Content.ReadAsAsync<Contact>();
        }

        public async Task<bool> ApproveUser(Guid contactId)
        {
            var result = await HttpClient.PostAsJsonAsync("/Account/{contactId}/approve", string.Empty);
            return result.IsSuccessStatusCode;
        }

        public async Task Callback(DfeSignInCallback callback)
        {
            await HttpClient.PostAsJsonAsync($"/Account/Callback", callback);
        }
    }
}