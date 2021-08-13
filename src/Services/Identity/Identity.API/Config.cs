// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace Identity.API
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
                   new IdentityResource[]
                   {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                   };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("Property.API"),
            };

        public static IEnumerable<Client> Clients(IConfiguration config) =>
            new Client[]
            {
                // m2m client credentials flow client
                new Client
                {
                    ClientId = "swagger",
                    ClientName = "Swagger",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowAccessTokensViaBrowser = true,
                    ClientSecrets = { new Secret("511536EF-F270-4058-80CA-1C89C192F69A".Sha256()) },
                    RedirectUris = {$"{config["PROPERTYMANAGEMENT_URL"]}/swagger/oauth2-redirect.html"},
                    AllowedScopes = { "Property.API" } ,
                    AllowedCorsOrigins = {$"{config["PROPERTYMANAGEMENT_URL"]}", $"{config["PROPERTYMANAGEMENTSWAGGERCLIENT"]}" }
                },

                // interactive client using code flow + pkce
                new Client
                {
                    ClientId = "angular-client",
                    ClientName = "Angular Client",
                    AllowedGrantTypes = GrantTypes.Code ,
                    RequirePkce = true,
                    RequireClientSecret = false,
                    AllowedScopes = { "openid", "profile", "Property.API" },
                    AccessTokenLifetime = 600,
                    RequireConsent = false,
                    AllowAccessTokensViaBrowser = true,
                    RedirectUris = new List<string>{$"{config["WEBAPP_URL"]}/signin-callback", $"{config["WEBAPP_URL"]}/assets/silent-callback.html" },
                    AllowedCorsOrigins = {$"{config["WEBAPP_URL"]}" },
                    PostLogoutRedirectUris = new List<string>{ $"{config["WEBAPP_URL"]}/signout-callback" }
                },
            };
    }
}