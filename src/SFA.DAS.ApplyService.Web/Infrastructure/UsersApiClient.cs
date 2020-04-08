using System;
using System.Net;
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

    public class UsersApiClient : ApiClientBase<UsersApiClient>, IUsersApiClient
    {
        public UsersApiClient(ILogger<UsersApiClient> logger, IConfigurationService configurationService, ITokenService tokenService) : base(logger)
        {
            if (_httpClient.BaseAddress == null)
            {
                _httpClient.BaseAddress = new Uri(configurationService.GetConfig().Result.InternalApi.Uri);
            }
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenService.GetToken());
        }

        public async Task<bool> InviteUser(CreateAccountViewModel vm)
        {
            var result = await Post($"/Account/", vm);
            return result == HttpStatusCode.OK;
        }

        public async Task<Contact> GetUserBySignInId(string signInId)
        {
            return await Get<Contact>($"/Account/{signInId}");
        }

        public async Task<bool> ApproveUser(Guid contactId)
        {
            var result = await Post($"/Account/{contactId}/approve", new { });
            return result == HttpStatusCode.OK;
        }

        public async Task Callback(SignInCallback callback)
        {
            await Post($"/Account/Callback", callback);
        }

        public async Task MigrateUsers()
        {
            await Post("/Account/MigrateUsers", new { });
        }

        public async Task AssociateOrganisationWithUser(Guid contactId, Guid organisationId)
        {
            await Post($"/Account/UpdateContactWithOrgId", new UpdateContactOrgId
            {
                ContactId = contactId,
                OrganisationId = organisationId
            });
        }

        public async Task MigrateContactAndOrgs(MigrateContactOrganisation migrateContactOrganisation)
        {
            await Post("/Account/MigrateContactAndOrgs", migrateContactOrganisation);
        }
    }
}