using System;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using SFA.DAS.ApplyService.Application;
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
           
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.DfeSignIn.ApiClientSecret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(issuer: config.DfeSignIn.ClientId, audience: "signin.education.gov.uk",
                signingCredentials: creds);
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
                httpClient.DefaultRequestHeaders.Add("Accept", "application/json");  

                var inviteJson = JsonConvert.SerializeObject(new
                {
                    sourceId = userId.ToString(),
                    given_name = givenName,
                    family_name = familyName,
                    email = email,
                    userRedirect = config.DfeSignIn.RedirectUri,
                    callback = config.DfeSignIn.CallbackUri
                });
                
                var dfeResponse = await httpClient.PostAsync(config.DfeSignIn.ApiUri,
                    new StringContent(inviteJson, Encoding.UTF8, "application/json")
                );

                if (!dfeResponse.IsSuccessStatusCode)
                {
                    var content = await dfeResponse.Content.ReadAsStringAsync();
                    _logger.LogError("Error from DfE Invitation Service. Status Code: {0}. Message: {0}",
                        (int) dfeResponse.StatusCode, content);
                    return new InviteUserResponse() {IsSuccess = false};
                }
                
               return new InviteUserResponse();
            }
        }
    }
}