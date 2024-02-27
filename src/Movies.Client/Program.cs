using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Movies.Client.ApiServices;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// add service
builder.Services.AddScoped<IMovieApiService, MovieApiService>();

builder.Services.AddAuthentication(opt => 
                 {
                     opt.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                     opt.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
                 })
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme , 
                  opt => 
                  {
                      opt.Authority = builder.Configuration.GetValue<string>("IdnetiyServerUrl");

                      opt.ClientId = builder.Configuration.GetValue<string>("client_id");
                      opt.ClientSecret = "secret";
                      opt.ResponseType = "code";

                      opt.Scope.Add("openid");
                      opt.Scope.Add("profile");

                      opt.SaveTokens = true;

                      opt.GetClaimsFromUserInfoEndpoint = true;
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
