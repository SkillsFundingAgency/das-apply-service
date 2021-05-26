﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SFA.DAS.ApplyService.Configuration;

namespace SFA.DAS.ApplyService.Web.Infrastructure
{
    public static class AuthorizationStartupExtensions
    {
        public static void AddDfeSignInAuthorization(this IServiceCollection services, IApplyConfig applyConfig, ILogger logger, IHostingEnvironment env)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddAuthentication(options =>
                {
                    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
                })
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
                {
                    options.Cookie.Name = ".Assessors.Cookies";
                    options.Cookie.HttpOnly = true;

                    if (!env.IsDevelopment())
                    {
                        options.Cookie.Domain = ".apprenticeships.education.gov.uk";   
                    }
                    
                    options.SlidingExpiration = true; 
                    options.ExpireTimeSpan = TimeSpan.FromHours(1);                    
                })
                .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
                {
                    options.CorrelationCookie = new CookieBuilder()
                    {
                        Name = ".Assessor.Correlation.", 
                        HttpOnly = true,
                        SameSite = SameSiteMode.None,
                        SecurePolicy = CookieSecurePolicy.SameAsRequest
                    };

                    options.Authority = applyConfig.DfeSignIn.MetadataAddress;
                    options.RequireHttpsMetadata = false;
                    options.ClientId = applyConfig.DfeSignIn.ClientId;
                    options.SaveTokens = true;
            
                    options.Scope.Clear();
                    options.Scope.Add("openid");
                    options.Scope.Add("profile");

                 
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

                        OnTokenValidated = async context =>
                        {
                            Guid.TryParse(context.Principal.FindFirstValue("sub"), out var signInId);

                            var client = context.HttpContext.RequestServices.GetRequiredService<IUsersApiClient>();       
                            var user = await client.GetUserBySignInId(signInId);
                            if (user != null)
                            {
                                if (user.Status == "Deleted")
                                {
                                    // Redirect to access denied page. 
                                    context.Response.Redirect("/Home/AccessDenied");
                                    context.HandleResponse();
                                }
                              
                                var primaryIdentity = context.Principal.Identities.FirstOrDefault();
                                if (primaryIdentity != null && string.IsNullOrEmpty(primaryIdentity.Name))
                                {
                                    primaryIdentity.AddClaim(new Claim(ClaimTypes.Name, $"{user.GivenNames} {user.FamilyName}"));
                                    // Note: In future, may want to consider populating the other Claims, such as Email
                                }

                                var identity = new ClaimsIdentity(new List<Claim>(){new Claim("UserId", user.Id.ToString())});                      
                                context.Principal.AddIdentity(identity);   
                            }   
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