using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SFA.DAS.ApplyService.Application.Interfaces;
using SFA.DAS.ApplyService.Configuration;

namespace SFA.DAS.ApplyService.DfeSignIn
{
    public class DfeSignInService : IDfeSignInService
    {
        private readonly IConfigurationService _configurationService;
        private readonly ILogger<DfeSignInService> _logger;

        public DfeSignInService(IConfigurationService configurationService, ILogger<DfeSignInService> logger)
        {
            _configurationService = configurationService;
            _logger = logger;
        }
        
        public async Task<InviteUserResponse> InviteUser(string email, string givenName, string familyName, Guid userId)
        {
            var config = await _configurationService.GetConfig();
           
            
            var client = new HttpClient();
            var disco = await client.GetDiscoveryDocumentAsync(config.DfeSignIn.MetadataAddress);
            if (disco.IsError)
            {
                Console.WriteLine(disco.Error);
            }
            
            // request token
            var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = "client",
                ClientSecret = config.DfeSignIn.ApiClientSecret,
                Scope = "api1"
            });

            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
            }

            Console.WriteLine(tokenResponse.Json);

            using (var httpClient = new HttpClient())
            {
                httpClient.SetBearerToken(tokenResponse.AccessToken);

            
                var inviteJson = JsonConvert.SerializeObject(new
                {
                    sourceId = userId.ToString(),
                    given_name = givenName,
                    family_name = familyName,
                    email = email,
                    userRedirect = config.DfeSignIn.RedirectUri,
                    callback = config.DfeSignIn.CallbackUri
                });
                
                var response = await httpClient.PostAsync(config.DfeSignIn.ApiUri,
                    new StringContent(inviteJson, Encoding.UTF8, "application/json")
                );
            
            
                var content = await response.Content.ReadAsStringAsync();
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };

                var responseObject = JsonConvert.DeserializeObject<CreateInvitationResponse>(content, settings);
                
                _logger.LogInformation("Returned from DfE Invitation Service. Status Code: {0}. Message: {0}",
                    (int) response.StatusCode, content);

                if (response.IsSuccessStatusCode)
                    return responseObject.Message == "User already exists"
                        ? new InviteUserResponse() {UserExists = true, IsSuccess = false, ExistingUserId = responseObject.ExistingUserId}
                        : new InviteUserResponse();
                
                _logger.LogError("Error from DfE Invitation Service. Status Code: {0}. Message: {0}",
                    (int) response.StatusCode, content);
                return new InviteUserResponse() {IsSuccess = false};
            }
        }
        
        private class CreateInvitationResponse
        {
            public string Message { get; set; }
            public bool Invited { get; set; }
            public Guid InvitationId { get; set; }
            public Guid ExistingUserId { get; set; }
        }
    }
}