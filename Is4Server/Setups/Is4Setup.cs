using System.Collections.Generic;
using IdentityServer4;
using IdentityServer4.Models;

namespace Is4Server.Setups
{
    public class Is4Setup
    {
        /// <summary>
        /// Load pre-defined identity resource.
        /// </summary>
        /// <returns></returns>
        internal static IEnumerable<IdentityResource> LoadIdentityResources()
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
        internal static IEnumerable<ApiResource> LoadApiResources()
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
        internal static IEnumerable<Client> LoadClients()
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
            authorizationCodeClient.AllowedGrantTypes = new List<string>();
            authorizationCodeClient.AllowedGrantTypes.Add(GrantType.AuthorizationCode);
            authorizationCodeClient.ClientSecrets = new List<Secret>();
            authorizationCodeClient.ClientSecrets.Add(new Secret("secret".Sha256()));
            authorizationCodeClient.RedirectUris = new List<string>();
            authorizationCodeClient.RedirectUris.Add("http://localhost:4300");
            authorizationCodeClient.PostLogoutRedirectUris = new List<string>();
            authorizationCodeClient.PostLogoutRedirectUris.Add("http://localhost:5002/signout-callback-oidc");
            authorizationCodeClient.RequirePkce = false;
            authorizationCodeClient.AllowedScopes = new List<string>();
            authorizationCodeClient.AllowedScopes.Add(IdentityServerConstants.StandardScopes.OpenId);
            authorizationCodeClient.AllowedScopes.Add(IdentityServerConstants.StandardScopes.Profile);
            authorizationCodeClient.AllowedScopes.Add("api1");
            authorizationCodeClient.AllowOfflineAccess = true;
            authorizationCodeClient.RequireConsent = true;
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
    }
}