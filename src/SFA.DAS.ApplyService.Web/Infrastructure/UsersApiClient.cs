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
        Task Callback(SignInCallback callback);
        Task AssociateOrganisationWithUser(Guid contactId, Guid organisationId);
        Task MigrateUsers();
        Task MigrateContactAndOrgs(MigrateContactOrganisation migrateContactOrganisation);
    }

    public class UsersApiClient : ApiClientBase, IUsersApiClient
    {
        private readonly ILogger<UsersApiClient> _logger;

        public UsersApiClient(ILogger<UsersApiClient> logger, IConfigurationService configService, ITokenService tokenService) : base(logger, configService, tokenService)
        {
            _logger = logger;
        }

        public async Task<bool> InviteUser(CreateAccountViewModel vm)
        {
            using (var result = await Post("/Account/", vm))
            {
                return result.IsSuccessStatusCode;
            }
        }

        public async Task<Contact> GetUserBySignInId(string signInId)
        {
            using (var httpResponseMessage = await Get($"/Account/{signInId}"))
            {
                var contactJson = await httpResponseMessage.Content.ReadAsStringAsync();

                _logger.LogInformation($"GetUserBySignInId result: {contactJson}");

                return JsonConvert.DeserializeObject<Contact>(contactJson);
            }
        }

        public async Task<bool> ApproveUser(Guid contactId)
        {
            using (var result = await Post($"/Account/{contactId}/approve", new { }))
            {
                return result.IsSuccessStatusCode;
            }
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
            await Put($"/Account/UpdateContactWithOrgId", new UpdateContactOrgId
            {
                ContactId = contactId,
                OrganisationId = organisationId
            });
        }

        public async Task MigrateContactAndOrgs(MigrateContactOrganisation migrateContactOrganisation)
        {
            await Post($"/Account/MigrateContactAndOrgs", migrateContactOrganisation);
        }
    }
}