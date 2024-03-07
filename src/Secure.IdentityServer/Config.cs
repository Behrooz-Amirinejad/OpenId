using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Security.Claims;

namespace Secure.IdentityServer;

public class Config
{
    public static IEnumerable<Client> Clients =>
        new Client[]
        {
            //new Client
            //{
            //    ClientId = "movieClient",
            //    AllowedGrantTypes = GrantTypes.ClientCredentials,
            //    ClientSecrets =
            //    {
            //        new Secret("secret".Sha256())
            //    },
            //    AllowedScopes = {"movieAPI"}
            //},
            new Client
            {
                ClientId = "movie_mvc_client",
                ClientName = "Movies MVC web App",
                AllowedGrantTypes = GrantTypes.Hybrid,
                RequirePkce = false,
                AllowRememberConsent = false,
                RedirectUris = new List<string>()
                {
                    "https://localhost:8003/signin-oidc"
                },
                PostLogoutRedirectUris = new List<string>()
                {
                    "https://localhost:8003/signout-callback-oidc"
                },
                ClientSecrets =
                {
                    new Secret("secret".Sha256())
                },
                AllowedScopes = new List<string>()
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityServerConstants.StandardScopes.Address,
                    IdentityServerConstants.StandardScopes.Email,
                    "movieAPI",
                    "roles"
                },
                AllowAccessTokensViaBrowser = true,
            }
        };

    public static IEnumerable<ApiScope> ApiScopes => new ApiScope[]
    {
        new ApiScope("movieAPI" , "Movie API")
    };

    public static IEnumerable<ApiResource> ApiResources => new ApiResource[] { };

    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
    {
        new IdentityResources.OpenId(),
        new IdentityResources.Profile(),
        new IdentityResources.Address(),
        new IdentityResources.Email(),
        new IdentityResource("roles",
                            "Your roles(s)",
                            new List<string>(){ "role" })
    };

    public static List<TestUser> TestUsers => new List<TestUser>
    {
        new TestUser()
        {
            SubjectId = "8640e118-8360-4a37-8929-18f394163996",
            Username = "ben",
            Password = "ben",
            Claims = new List<Claim>
            {
                new Claim(JwtClaimTypes.GivenName , "Behrooz"),
                new Claim(JwtClaimTypes.FamilyName , "Amiri"),
                new Claim(JwtClaimTypes.Role , "Manager")
            }
        }
    };

}
