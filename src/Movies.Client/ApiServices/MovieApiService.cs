using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Movies.Client.Models;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Movies.Client.ApiServices;

public class MovieApiService : IMovieApiService
{
    private readonly IConfiguration _configuration;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public MovieApiService(IConfiguration configuration, IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
    {
        this._configuration = configuration;
        _httpClientFactory = httpClientFactory;
        _httpContextAccessor = httpContextAccessor;
    }
    public async Task<IEnumerable<Movie>> GetMovie()
    {
        var httpClient = _httpClientFactory.CreateClient("MovieAPIClient");
        var request = new HttpRequestMessage(HttpMethod.Get,
                                                            "/api/movies/getmovies");

        var response = await httpClient.SendAsync(request,
                                                    HttpCompletionOption.ResponseHeadersRead);

        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        var movieList = JsonConvert.DeserializeObject<List<Movie>>(content);
        return movieList;

        #region Old generation code
        /*

                // 1 get token from identityt server
                string idnetityServerURL = _configuration.GetValue<string>("IdnetiyServerUrl");
                var apiClientCredentials = new ClientCredentialsTokenRequest() 
                {
                    Address =  $"{idnetityServerURL}/connect/token",
                    ClientId = "movieClient",
                    ClientSecret = "secret",
                    Scope = "movieAPI",
                    GrantType = "client_credentials"
                };

                var client = new HttpClient();
                var disc = await client.GetDiscoveryDocumentAsync(idnetityServerURL);
                if (disc.IsError)
                    throw new Exception("Can not connect to Identity server!");

                // 2 authenticate and get the credential
                var tokenResponse = await client.RequestClientCredentialsTokenAsync(apiClientCredentials);

                if (tokenResponse.IsError)
                    throw new Exception("bad request by client credential token");
                Console.WriteLine($"Token: {tokenResponse.AccessToken}");

                // 3 send the request to Movie API
                var movieApi = new HttpClient();
                movieApi.SetBearerToken(tokenResponse.AccessToken);

                var movieApiResponse = await movieApi.GetAsync( $"{_configuration.GetValue<string>("movieServiceUrl")}/api/movies/getmovies");
                movieApiResponse.EnsureSuccessStatusCode();
                var responseContent = await movieApiResponse.Content.ReadAsStringAsync();

                // 4 convert the result in movie list
                var movieList = JsonConvert.DeserializeObject<List<Movie>>(responseContent);
                return movieList;
        */
        #endregion
    }

    public Task<Movie> CreateMovie(Movie movie)
    {
        throw new NotImplementedException();
    }

    public Task DeleteMovie(int id)
    {
        throw new NotImplementedException();
    }


    public Task<Movie> GetMovie(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Movie> UpdateMovie(Movie movie)
    {
        throw new NotImplementedException();
    }

    public async Task<UserInfoViewModel> GetUserInfo()
    {
        var idpClient = _httpClientFactory.CreateClient("IDPClient");

        var metaDataRespinse = await idpClient.GetDiscoveryDocumentAsync();
        if (metaDataRespinse.IsError)
            throw new HttpRequestException("Somethin went wrong while requesting the access token!");

        var accessToken = await _httpContextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);

        var userInfo = await idpClient.GetUserInfoAsync(
            new UserInfoRequest()
            {
                Address = metaDataRespinse.UserInfoEndpoint,
                Token = accessToken
            });

        if (userInfo.IsError)
            throw new HttpRequestException("Somethin went wrong while requesting while getting user info");

        var userInfoDictionary = new Dictionary<string, string>();
        userInfo.Claims.ToList()
                        .ForEach(claim =>
                        {
                            userInfoDictionary.Add(claim.Type, claim.Value);
                        });

        return new UserInfoViewModel(userInfoDictionary);

    }
}
