using System;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using SFA.DAS.ApplyService.Application;
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
            var disco = await client.GetDiscoveryDocumentAsync("https://localhost:5001");
            if (disco.IsError)
            {
                Console.WriteLine(disco.Error);
            }
            
            // request token
            var tokenResponse = client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,

                ClientId = "client",
                ClientSecret = config.DfeSignIn.ApiClientSecret,//"511536EF-F270-4058-80CA-1C89C192F69A",
                Scope = "api1"
            }).Result;

            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
            }

            Console.WriteLine(tokenResponse.Json);
            
//            
//            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.DfeSignIn.ApiClientSecret));
//            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
//
//            var token = new JwtSecurityToken(issuer: config.DfeSignIn.ClientId, audience: "signin.education.gov.uk",
//                signingCredentials: creds);
//            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

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
            
            
//                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
//                httpClient.DefaultRequestHeaders.Add("Accept", "application/json");  
//
//                var inviteJson = JsonConvert.SerializeObject(new
//                {
//                    sourceId = userId.ToString(),
//                    given_name = givenName,
//                    family_name = familyName,
//                    email = email,
//                    userRedirect = config.DfeSignIn.RedirectUri,
//                    callback = config.DfeSignIn.CallbackUri
//                });
//                
//                var dfeResponse = await httpClient.PostAsync(config.DfeSignIn.ApiUri,
//                    new StringContent(inviteJson, Encoding.UTF8, "application/json")
//                );
//
                var content = await response.Content.ReadAsStringAsync();
                
                _logger.LogInformation("Returned from DfE Invitation Service. Status Code: {0}. Message: {0}",
                    (int) response.StatusCode, content);
                
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Error from DfE Invitation Service. Status Code: {0}. Message: {0}",
                        (int) response.StatusCode, content);
                    return new InviteUserResponse() {IsSuccess = false};
                }
                
               return new InviteUserResponse();
            }
        }
    }
}