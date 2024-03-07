using IdentityModel;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Movies.Client.ApiServices;
using Movies.Client.HttpHandlers;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// add service
builder.Services.AddScoped<IMovieApiService, MovieApiService>();

string identityServiceURL = builder.Configuration.GetValue<string>("identityServiceUrl");

builder.Services.AddAuthentication(opt =>
                 {
                     opt.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                     opt.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
                 })
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme,
                  opt =>
                  {
                      opt.Authority = identityServiceURL;

                      opt.ClientId = builder.Configuration.GetValue<string>("client_id");
                      opt.ClientSecret = "secret";
                      opt.ResponseType = "code id_token";

                      opt.Scope.Add("openid");
                      opt.Scope.Add("address");
                      opt.Scope.Add("email");
                      opt.Scope.Add("profile");
                      opt.Scope.Add("movieAPI");
                      opt.Scope.Add("roles");
                      opt.ClaimActions.MapUniqueJsonKey("role", "role");

                      opt.SaveTokens = true;

                      opt.GetClaimsFromUserInfoEndpoint = true;
                      opt.TokenValidationParameters = new TokenValidationParameters()
                      {
                          NameClaimType = JwtClaimTypes.GivenName,
                          RoleClaimType = JwtClaimTypes.Role,
                      };
                  });

builder.Services.AddTransient<AuthenticationDelegatingHandler>();

builder.Services.AddHttpClient("MovieAPIClient", client =>
{
    client.BaseAddress = new Uri(builder.Configuration.GetValue<string>("movieServiceUrl"));
    client.DefaultRequestHeaders.Clear();
    client.DefaultRequestHeaders.Add("Accept", "application/json");
}).AddHttpMessageHandler<AuthenticationDelegatingHandler>();

builder.Services.AddHttpClient("IDPClient", client =>
{
    client.BaseAddress = new Uri(identityServiceURL);
    client.DefaultRequestHeaders.Clear();
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

builder.Services.AddHttpContextAccessor();

builder.Services.AddSingleton(new ClientCredentialsTokenRequest()
{
    Address = $"{identityServiceURL}/connect/token",
    ClientId = "movieClient",
    ClientSecret = "secret",
    Scope = "movieAPI",
    GrantType = "client_credentials"
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
