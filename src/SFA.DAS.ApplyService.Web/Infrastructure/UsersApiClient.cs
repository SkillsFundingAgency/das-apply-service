using System;
using System.Net.Http;
using System.Net.Http.Headers;
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
        Task Callback(SignInCallback callback);
        Task AssociateOrganisationWithUser(Guid contactId, Guid organisationId);
        Task MigrateUsers();
        Task MigrateContactAndOrgs(MigrateContactOrganisation migrateContactOrganisation);
    }

    public class UsersApiClient : IUsersApiClient
    {
        private readonly ILogger<UsersApiClient> _logger;
        private readonly ITokenService _tokenService;
        private static readonly HttpClient _httpClient = new HttpClient();

        public UsersApiClient(IConfigurationService configurationService, ILogger<UsersApiClient> logger, ITokenService tokenService)
        {
            _logger = logger;
            _tokenService = tokenService;

            if (_httpClient.BaseAddress == null)
            {
                _httpClient.BaseAddress = new Uri(configurationService.GetConfig().Result.InternalApi.Uri);
            }
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _tokenService.GetToken());
        }

        public async Task<bool> InviteUser(CreateAccountViewModel vm)
        {
            var result = await _httpClient.PostAsJsonAsync("/Account/", vm);
            return result.IsSuccessStatusCode;
        }

        public async Task<Contact> GetUserBySignInId(string signInId)
        {
            var httpResponseMessage = await _httpClient.GetAsync($"/Account/{signInId}");

            var contactJson = await httpResponseMessage.Content.ReadAsStringAsync();

            _logger.LogInformation($"GetUserBySignInId result: {contactJson}");
            
            return JsonConvert.DeserializeObject<Contact>(contactJson);
        }

        public async Task<bool> ApproveUser(Guid contactId)
        {
            var result = await _httpClient.PostAsJsonAsync("/Account/{contactId}/approve", string.Empty);
            return result.IsSuccessStatusCode;
        }

        public async Task Callback(SignInCallback callback)
        {
            await _httpClient.PostAsJsonAsync($"/Account/Callback", callback);
        }

        public async Task MigrateUsers()
        {
            await _httpClient.PostAsync("/Account/MigrateUsers", new StringContent(""));
        }

        public async Task AssociateOrganisationWithUser(Guid contactId, Guid organisationId)
        {
            await _httpClient.PutAsJsonAsync($"/Account/UpdateContactWithOrgId", new UpdateContactOrgId
            {
                ContactId=contactId,
                OrganisationId=organisationId
            } );
        }

        public async Task MigrateContactAndOrgs(MigrateContactOrganisation migrateContactOrganisation)
        {
            await _httpClient.PostAsJsonAsync($"/Account/MigrateContactAndOrgs", migrateContactOrganisation);
        }
    }
}