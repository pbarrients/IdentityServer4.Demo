﻿using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace IdentityServer4Demo
{
    public class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
            };
        }

        public static IEnumerable<ApiScope> GetApiScopes()
        {
            return new List<ApiScope>
            {
                // backward compat
                new ApiScope("api"),
                
                // more formal
                new ApiScope("api.scope1"),
                new ApiScope("api.scope2"),
                
                // scope without a resource
                new ApiScope("scope2"),
                
                // policyserver
                new ApiScope("policyserver.runtime"),
                new ApiScope("policyserver.management"),

                new ApiScope(IdentityServerConstants.LocalApi.ScopeName)
            };
        }
        
        public static IEnumerable<ApiResource> GetApis()
        {
            return new List<ApiResource>
            {
                new ApiResource("api", "Demo API")
                {
                    ApiSecrets = { new Secret("secret".Sha256()) },
                    
                    Scopes = { "api", "api.scope1", "api.scope2" }
                },
                new ApiResource(IdentityServerConstants.LocalApi.ScopeName)
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                // non-interactive
                new Client
                {
                    ClientId = "m2m",
                    ClientName = "Machine to machine (client credentials)",
                    ClientSecrets = { new Secret("secret".Sha256()) },
                    AccessTokenType = AccessTokenType.Reference,
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = {
                        "api",
                        "api.scope1",
                        "api.scope2", "scope2",
                        "policyserver.runtime",
                        "policyserver.management",
                        IdentityServerConstants.LocalApi.ScopeName
                    },
                },
                new Client
                {
                    ClientId = "m2m.short",
                    ClientName = "Machine to machine with short access token lifetime (client credentials)",
                    ClientSecrets = { new Secret("secret".Sha256()) },

                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = { "api", "api.scope1", "api.scope2", "scope2" },
                    AccessTokenLifetime = 75
                },

                // interactive
                new Client
                {
                    ClientId = "interactive.confidential",
                    ClientName = "Interactive client (Code with PKCE)",

                    RedirectUris = { "https://notused" },
                    PostLogoutRedirectUris = { "https://notused" },

                    ClientSecrets = { new Secret("secret".Sha256()) },

                    AllowedGrantTypes = GrantTypes.CodeAndClientCredentials,
                    AllowedScopes = { "openid", "profile", "email", "api", "api.scope1", "api.scope2", "scope2" },

                    AllowOfflineAccess = true,
                    RefreshTokenUsage = TokenUsage.ReUse,
                    RefreshTokenExpiration = TokenExpiration.Sliding
                },
                new Client
                {
                    ClientId = "interactive.confidential.hybrid",
                    ClientName = "Interactive client (OIDC Hybrid Flow)",

                    RedirectUris = { "https://notused" },
                    PostLogoutRedirectUris = { "https://notused" },

                    ClientSecrets = { new Secret("secret".Sha256()) },

                    AllowedGrantTypes = GrantTypes.HybridAndClientCredentials,
                    AllowedScopes = { "openid", "profile", "email", "api", "api.scope1", "api.scope2", "scope2" },

                    AllowOfflineAccess = true,
                    RefreshTokenUsage = TokenUsage.ReUse,
                    RefreshTokenExpiration = TokenExpiration.Sliding
                },
                new Client
                {
                    ClientId = "interactive.confidential.short",
                    ClientName = "Interactive client with short token lifetime (Code with PKCE)",

                    RedirectUris = { "https://notused" },
                    PostLogoutRedirectUris = { "https://notused" },

                    ClientSecrets = { new Secret("secret".Sha256()) },
                    RequireConsent = false,

                    AllowedGrantTypes = GrantTypes.CodeAndClientCredentials,
                    RequirePkce = true,
                    AllowedScopes = { "openid", "profile", "email", "api", "api.scope1", "api.scope2", "scope2" },

                    AllowOfflineAccess = true,
                    RefreshTokenUsage = TokenUsage.ReUse,
                    RefreshTokenExpiration = TokenExpiration.Sliding,
                    
                    AccessTokenLifetime = 75
                },

                new Client
                {
                    ClientId = "interactive.public",
                    ClientName = "Interactive client (Code with PKCE)",

                    RedirectUris = { "https://notused" },
                    PostLogoutRedirectUris = { "https://notused" },

                    RequireClientSecret = false,

                    AllowedGrantTypes = GrantTypes.Code,
                    AllowedScopes = { "openid", "profile", "email", "api", "api.scope1", "api.scope2", "scope2" },

                    AllowOfflineAccess = true,
                    RefreshTokenUsage = TokenUsage.OneTimeOnly,
                    RefreshTokenExpiration = TokenExpiration.Sliding
                },
                new Client
                {
                    ClientId = "interactive.public.short",
                    ClientName = "Interactive client with short token lifetime (Code with PKCE)",

                    RedirectUris = { "https://notused" },
                    PostLogoutRedirectUris = { "https://notused" },

                    RequireClientSecret = false,

                    AllowedGrantTypes = GrantTypes.Code,
                    AllowedScopes = { "openid", "profile", "email", "api", "api.scope1", "api.scope2", "scope2" },

                    AllowOfflineAccess = true,
                    RefreshTokenUsage = TokenUsage.OneTimeOnly,
                    RefreshTokenExpiration = TokenExpiration.Sliding,
                    
                    AccessTokenLifetime = 75
                },

                new Client
                {
                    ClientId = "device",
                    ClientName = "Device Flow Client",

                    AllowedGrantTypes = GrantTypes.DeviceFlow,
                    RequireClientSecret = false,

                    AllowOfflineAccess = true,
                    RefreshTokenUsage = TokenUsage.OneTimeOnly,
                    RefreshTokenExpiration = TokenExpiration.Sliding,
                    
                    AllowedScopes = { "openid", "profile", "email", "api", "api.scope1", "api.scope2", "scope2" }
                },
                
                // oidc login only
                new Client
                {
                    ClientId = "login",
                    
                    RedirectUris = { "https://notused" },
                    PostLogoutRedirectUris = { "https://notused" },
                    
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowedScopes = { "openid", "profile", "email" },
                },
                
                new Client
                {
                    ClientId = "hybrid",
                    ClientSecrets = { new Secret("secret".Sha256()) },
                    
                    AllowedGrantTypes = GrantTypes.Hybrid,
                    RequirePkce = false,

                    RedirectUris = { "https://notused" },
                    PostLogoutRedirectUris = { "https://notused" },

                    AllowOfflineAccess = true,
                    AllowedScopes = { "openid", "profile", "email", "api", "api.scope1", "api.scope2", "scope2" }
                }
            };
        }
    }
}
