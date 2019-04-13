using System.Collections.Generic;
using System.Reflection;
using IdentityServer4;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Models;
using Is4Server.Services.Implementations;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Is4Server.Setups
{
    public class AuthenticationSetup
    {
        #region Methods

        /// <summary>
        /// Run authentication setup.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void Run(IServiceCollection services, IConfiguration configuration)
        {
            const string connectionString = @"Data Source=.\SQLEXPRESS;database=Is4.LearnIs4;trusted_connection=yes;";
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            services
                .AddIdentityServer()
#if USE_SQL
                //.AddProfileService<IdentityServerProfileService>()
                //.AddResourceOwnerValidator<ResourceOwnerPasswordValidator>()
                // this adds the config data from DB (clients, resources)
                .AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = builder =>
                    {
                        builder.UseSqlServer(connectionString,
                            sql => sql.MigrationsAssembly(migrationsAssembly));
                    };
                })
                // this adds the operational data from DB (codes, tokens, consents)
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = builder =>
                        builder.UseSqlServer(connectionString,
                            sql => sql.MigrationsAssembly(migrationsAssembly));
                    // this enables automatic token cleanup. this is optional.
                    options.EnableTokenCleanup = true;
                })
#else
                .AddInMemoryIdentityResources(LoadIdentityResources())
                .AddInMemoryApiResources(LoadApiResources())
                .AddInMemoryClients(LoadClients())
                .AddInMemoryPersistedGrants()
#endif
                .AddDeveloperSigningCredential();

            // Add jwt validation.
            services
                .AddAuthentication()
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = "http://localhost:57547";
                    options.ApiName = "api1";
                    options.ApiSecret = "secret";
                    options.RequireHttpsMetadata = false;
                })
                //                .AddJwtBearer(options =>
                //                {
                //                    // base-address of your identityserver
                //                    options.Authority = "http://localhost:57547";

                //                    // name of the API resource
                //                    options.Audience = "api1";

                //                    options.RequireHttpsMetadata = false;

                //                    // You also need to update /wwwroot/app/scripts/app.js
                //                    options.SecurityTokenValidators.Clear();
                //                    options.SecurityTokenValidators.Add(new Defau());

                //                    // Initialize token validation parameters.
                //                    var tokenValidationParameters = new TokenValidationParameters();
                //                    tokenValidationParameters.ValidAudience = jwtOptions.Audience;
                //                    tokenValidationParameters.ValidIssuer = jwtOptions.Issuer;
                //                    tokenValidationParameters.IssuerSigningKey = jwtOptions.SigningKey;

                //#if DEBUG
                //                    tokenValidationParameters.ValidateLifetime = false;
                //#endif

                //                    o.TokenValidationParameters = tokenValidationParameters;

                //                })
                .AddGoogle("Google", options =>
                {
                    options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;

                    options.ClientId = "323676358406-ikvol20relacv3mn5popdi79e5m759pc.apps.googleusercontent.com";
                    options.ClientSecret = "68pGK3guMhv_bdJKQOznblSi";

                    options.SaveTokens = true;
                    options.AccessType = "offline";
                });
            //.AddOpenIdConnect("demoidsrv", "IdentityServer", options =>
            //{
            //    options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
            //    options.SignOutScheme = IdentityServerConstants.SignoutScheme;

            //    options.Authority = "http://localhost:57547";
            //    options.ClientId = "implicit";
            //    //options.ClientId = "323676358406-ikvol20relacv3mn5popdi79e5m759pc.apps.googleusercontent.com";
            //    //options.ClientSecret = "68pGK3guMhv_bdJKQOznblSi";
            //    options.ResponseType = "id_token";
            //    options.SaveTokens = true;
            //    options.CallbackPath = new PathString("/signin-idsrv");
            //    options.SignedOutCallbackPath = new PathString("/signout-callback-idsrv");
            //    options.RemoteSignOutPath = new PathString("/signout-idsrv");
            //    options.RequireHttpsMetadata = false;

            //    options.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        NameClaimType = "name",
            //        RoleClaimType = "role"
            //    };
            //});
        }

        /// <summary>
        /// Seed database.
        /// </summary>
        /// <param name="app"></param>
        public static void Seed(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();

                var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
                context.Database.Migrate();
                if (!context.Clients.Any())
                {
                    foreach (var client in LoadClients())
                        context.Clients.Add(client.ToEntity());
                    context.SaveChanges();
                }

                if (!context.IdentityResources.Any())
                {
                    foreach (var resource in LoadIdentityResources())
                        context.IdentityResources.Add(resource.ToEntity());
                    context.SaveChanges();
                }

                if (!context.ApiResources.Any())
                {
                    foreach (var resource in LoadApiResources())
                        context.ApiResources.Add(resource.ToEntity());
                    context.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Load pre-defined identity resource.
        /// </summary>
        /// <returns></returns>
        private static IEnumerable<IdentityResource> LoadIdentityResources()
        {
            var identityResources = new List<IdentityResource>();
            identityResources.Add(new IdentityResources.OpenId());
            identityResources.Add(new IdentityResources.Profile());

            return identityResources;
        }

        /// <summary>
        /// Load pre-defined api resources.
        /// </summary>
        /// <returns></returns>
        private static IEnumerable<ApiResource> LoadApiResources()
        {
            var apiResources = new List<ApiResource>();

            var api1Resource = new ApiResource("api1", "My API");
            api1Resource.ApiSecrets = new List<Secret>();
            api1Resource.ApiSecrets.Add(new Secret("secret".Sha256()));
            apiResources.Add(api1Resource);

            return apiResources;
        }

        /// <summary>
        /// Load pre-defined clients.
        /// </summary>
        /// <returns></returns>
        private static IEnumerable<Client> LoadClients()
        {
            var clients = new List<Client>();

            var clientCredentialClient = new Client();
            clientCredentialClient.ClientId = "client";
            clientCredentialClient.AllowedGrantTypes = GrantTypes.ClientCredentials;
            clientCredentialClient.ClientSecrets = new List<Secret>();
            clientCredentialClient.ClientSecrets.Add(new Secret("secret".Sha256()));
            clientCredentialClient.AllowedScopes = new List<string>();
            clientCredentialClient.AllowedScopes.Add("api1");
            clients.Add(clientCredentialClient);

            var resourceOwnerPasswordClient = new Client();
            resourceOwnerPasswordClient.ClientId = "ro.client";
            resourceOwnerPasswordClient.AllowedGrantTypes = new List<string>();
            resourceOwnerPasswordClient.AllowedGrantTypes.Add(GrantType.ResourceOwnerPassword);
            resourceOwnerPasswordClient.AllowedGrantTypes.Add("refresh_token");
            resourceOwnerPasswordClient.ClientSecrets = new List<Secret>();
            resourceOwnerPasswordClient.ClientSecrets.Add(new Secret("secret".Sha256()));
            resourceOwnerPasswordClient.AllowedScopes = new List<string>();
            resourceOwnerPasswordClient.AllowedScopes.Add("api1");
            resourceOwnerPasswordClient.AllowOfflineAccess = true;
            resourceOwnerPasswordClient.RefreshTokenExpiration = TokenExpiration.Sliding;
            resourceOwnerPasswordClient.RefreshTokenUsage = TokenUsage.ReUse;
            resourceOwnerPasswordClient.SlidingRefreshTokenLifetime = 3600;
            clients.Add(resourceOwnerPasswordClient);

            var authorizationCodeClient = new Client();
            authorizationCodeClient.ClientId = "mvc";
            authorizationCodeClient.ClientName = "MVC Client";
            //authorizationCodeClient.AllowedGrantTypes = new List<string>();
            //authorizationCodeClient.AllowedGrantTypes.Add(GrantType.AuthorizationCode);
            authorizationCodeClient.AllowedGrantTypes = GrantTypes.Hybrid;
            authorizationCodeClient.ClientSecrets = new List<Secret>();
            authorizationCodeClient.ClientSecrets.Add(new Secret("secret".Sha256()));
            authorizationCodeClient.ClientSecrets.Add(new Secret("68pGK3guMhv_bdJKQOznblSi".Sha256()));
            authorizationCodeClient.RedirectUris = new List<string>();
            authorizationCodeClient.RedirectUris.Add("http://localhost:4300");
            authorizationCodeClient.RedirectUris.Add("http://localhost:57547/signin-idsrv");
            authorizationCodeClient.RedirectUris.Add("http://localhost:57547/signin-google");
            authorizationCodeClient.RedirectUris.Add("http://127.0.0.1/sample-wpf-app");
            authorizationCodeClient.RedirectUris.Add("https://oidcdebugger.com/debug");
            authorizationCodeClient.RedirectUris.Add("http://localhost:57547/api/external-login");
            authorizationCodeClient.PostLogoutRedirectUris = new List<string>();
            authorizationCodeClient.PostLogoutRedirectUris.Add("http://localhost:5002/signout-callback-oidc");
            authorizationCodeClient.RequirePkce = false;
            authorizationCodeClient.AllowedScopes = new List<string>();
            authorizationCodeClient.AllowedScopes.Add(IdentityServerConstants.StandardScopes.OpenId);
            authorizationCodeClient.AllowedScopes.Add(IdentityServerConstants.StandardScopes.Profile);
            authorizationCodeClient.AllowedScopes.Add("api1");
            authorizationCodeClient.AllowOfflineAccess = true;
            authorizationCodeClient.RequireConsent = false;
            authorizationCodeClient.AllowAccessTokensViaBrowser = true;
            authorizationCodeClient.AccessTokenType = AccessTokenType.Reference;
            clients.Add(authorizationCodeClient);


            var codeClient = new Client();
            codeClient.ClientId = "js";
            codeClient.ClientName = "JavaScript Client";
            codeClient.AllowedGrantTypes = GrantTypes.Code;
            codeClient.RequirePkce = true;
            codeClient.RequireClientSecret = false;
            codeClient.RedirectUris = new List<string>();
            codeClient.RedirectUris.Add("http://localhost:5003/callback.html");
            codeClient.PostLogoutRedirectUris = new List<string>();
            codeClient.PostLogoutRedirectUris.Add("http://localhost:5003/index.html");
            codeClient.AllowedCorsOrigins = new List<string>();
            codeClient.AllowedCorsOrigins.Add("http://localhost:5003");
            codeClient.AllowedScopes = new List<string>();
            codeClient.AllowedScopes.Add(IdentityServerConstants.StandardScopes.OpenId);
            codeClient.AllowedScopes.Add(IdentityServerConstants.StandardScopes.Profile);
            codeClient.AllowedScopes.Add("api1");
            clients.Add(codeClient);

            return clients;
        }

        #endregion
    }
}