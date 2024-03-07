using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Net.Http;

namespace Movies.Client.HttpHandlers;

public class AuthenticationDelegatingHandler : DelegatingHandler
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ClientCredentialsTokenRequest _tokenRequest;

    //public AuthenticationDelegatingHandler(IHttpClientFactory httpClientFactory, ClientCredentialsTokenRequest clientCredentialsTokenRequest)
    //{
    //    _httpClientFactory = httpClientFactory;
    //    _tokenRequest = clientCredentialsTokenRequest;
    //}

    public AuthenticationDelegatingHandler(IHttpContextAccessor httpContextAccessor)
    {
        this._httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
    }


    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        string acctoken;

        //acctoken = await GetCredentialTokenRequest();
        acctoken = await HybridFlowGetAccessToken();

        if (!string.IsNullOrWhiteSpace(acctoken))
            request.SetBearerToken(acctoken);

        return await base.SendAsync(request, cancellationToken);
    }

    private async Task<string?> GetCredentialTokenRequest()
    {
        var httpclient = _httpClientFactory.CreateClient("idpclient");
        var tokenresponse = await httpclient.RequestClientCredentialsTokenAsync(_tokenRequest);

        if (tokenresponse.IsError)
            throw new HttpRequestException("something went wrong while requesting the access token");

        return tokenresponse.AccessToken;
    }

    private async Task<string?> HybridFlowGetAccessToken() 
    {
        var accessToken = await _httpContextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);

        return accessToken;

    }



} 
