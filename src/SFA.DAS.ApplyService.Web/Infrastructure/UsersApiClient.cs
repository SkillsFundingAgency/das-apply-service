using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Infrastructure.ApiClients;
using SFA.DAS.ApplyService.InternalApi.Types;
using SFA.DAS.ApplyService.Web.ViewModels;

namespace SFA.DAS.ApplyService.Web.Infrastructure
{
    public interface IUsersApiClient
    {
        Task<bool> InviteUser(CreateAccountViewModel vm);

        Task<bool> CreateUserFromAsLogin(Guid signInId, string email, string givenName, string familyName,
            string govUkIdentifier, Guid? userId);

        Task<Contact> GetUserBySignInId(Guid signInId);

        Task Callback(SignInCallback callback);

        Task<Contact> GetUserByEmail(string email);
    }

    public class UsersApiClient : ApiClientBase<UsersApiClient>, IUsersApiClient
    {
        public UsersApiClient(HttpClient httpClient, ILogger<UsersApiClient> logger, ITokenService tokenService) : base(httpClient, logger)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenService.GetToken());
        }

        public async Task<bool> InviteUser(CreateAccountViewModel vm)
        {
            var result = await Post($"/Account/", vm);
            return result == HttpStatusCode.OK;
        }

        public async Task<bool> CreateUserFromAsLogin(Guid signInId, string email, string givenName, string familyName,
            string govUkIdentifier, Guid? userId)
        {
            var contact = new NewContact { Email = email, GivenName = givenName, FamilyName = familyName, GovUkIdentifier = govUkIdentifier , UserId = userId};

            var result = await Post($"/Account/{signInId}", contact);
            return result == HttpStatusCode.OK;
        }

        public async Task<Contact> GetUserBySignInId(Guid signInId)
        {
            return await Get<Contact>($"/Account/{signInId}");
        }

        public async Task Callback(SignInCallback callback)
        {
            await Post($"/Account/Callback", callback);
        }

        public async Task<Contact> GetUserByEmail(string email)
        {
            return await Get<Contact>($"/Account/Contact/{HttpUtility.UrlEncode(email)}");
        }
    }
}