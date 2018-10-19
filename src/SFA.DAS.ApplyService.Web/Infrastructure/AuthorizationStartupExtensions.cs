using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using SFA.DAS.ApplyService.Configuration;

namespace SFA.DAS.ApplyService.Web.Infrastructure
{
    public static class AuthorizationStartupExtensions
    {
        public static void AddDfeSignInAuthorization(this IServiceCollection services)
        {
            services.AddAuthentication(options =>
                {
                    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
                })
                .AddCookie()
                .AddOpenIdConnect(options =>
                {
                    options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.MetadataAddress = "https://signin-test-oidc-as.azurewebsites.net/.well-known/openid-configuration";

                    options.ClientId = "DasAssessorServivce";
                    const string envKeyClientSecret = "tussock-sentient-onshore";
                    var clientSecret = "tussock-sentient-onshore";
                    if (string.IsNullOrWhiteSpace(clientSecret))
                    {
                        throw new Exception("Missing environment variable " + envKeyClientSecret +
                                            " - get this from the DfE Sign-in team.");
                    }

                    options.ClientSecret = clientSecret;
                    options.ResponseType = OpenIdConnectResponseType.Code;
                    options.GetClaimsFromUserInfoEndpoint = true;

                    // using this property would align the expiration of the cookie
                    // with the expiration of the identity token
                    // UseTokenLifetime = true;

                    options.Scope.Clear();
                    options.Scope.Add("openid");
                    options.Scope.Add("email");
                    options.Scope.Add("profile");

                    options.Scope.Add("offline_access");

                    options.SaveTokens = true;
                    //options.CallbackPath = new PathString(Configuration["auth:oidc:callbackPath"]);
                    //options.SignedOutCallbackPath = new PathString(Configuration["auth:oidc:signedOutCallbackPath"]);
                    options.SecurityTokenValidator = new JwtSecurityTokenHandler
                    {
                        InboundClaimTypeMap = new Dictionary<string, string>(),
                        TokenLifetimeInMinutes = 20,
                        SetDefaultTimesOnTokenCreation = true,
                    };
                    options.ProtocolValidator = new OpenIdConnectProtocolValidator
                    {
                        RequireSub = true,
                        RequireStateValidation = false,
                        NonceLifetime = TimeSpan.FromMinutes(15)
                    };

                    options.DisableTelemetry = true;
                    options.Events = new OpenIdConnectEvents
                    {
                        // Sometimes, problems in the OIDC provider (such as session timeouts)
                        // Redirect the user to the /auth/cb endpoint. ASP.NET Core middleware interprets this by default
                        // as a successful authentication and throws in surprise when it doesn't find an authorization code.
                        // This override ensures that these cases redirect to the root.
                        OnMessageReceived = context =>
                        {
                            var isSpuriousAuthCbRequest =
                                context.Request.Path == options.CallbackPath &&
                                context.Request.Method == "GET" &&
                                !context.Request.Query.ContainsKey("code");

                            if (isSpuriousAuthCbRequest)
                            {
                                context.HandleResponse();
                                context.Response.StatusCode = 302;
                                context.Response.Headers["Location"] = "/";
                            }

                            return Task.CompletedTask;
                        },

                        // Sometimes the auth flow fails. The most commonly observed causes for this are
                        // Cookie correlation failures, caused by obscure load balancing stuff.
                        // In these cases, rather than send user to a 500 page, prompt them to re-authenticate.
                        // This is derived from the recommended approach: https://github.com/aspnet/Security/issues/1165
                        OnRemoteFailure = ctx =>
                        {
                            ctx.Response.Redirect("/");
                            ctx.HandleResponse();
                            return Task.FromResult(0);
                        }
                    };
                });
        }
    }
}