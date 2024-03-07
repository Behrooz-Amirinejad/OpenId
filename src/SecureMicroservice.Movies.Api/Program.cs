using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SecureMicroservice.Movies.Api.Data;
using System.Configuration;
using System.Diagnostics;
using System.Drawing.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseInMemoryDatabase("Movie");
});

builder.Services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", option =>
                {
                    option.Authority = builder.Configuration.GetValue<string>("IdnetiyServerUrl"); ;

                    option.TokenValidationParameters = new TokenValidationParameters() { ValidateAudience = false };
                    //option.RequireHttpsMetadata = false;
                });
builder.Services.AddAuthorization(opt =>
{
    opt.AddPolicy("clientIdPlolicy",
                    policy =>
                            {
                                policy.RequireClaim("client_id", ["movieClient", "movie_mvc_client"]);
                            });

});

//Console.WriteLine(builder.Configuration.GetValue<string>("IdnetiyServerUrl"));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var provider = scope.ServiceProvider;
    var movieContext = provider.GetRequiredService<AppDbContext>();
    await MovieContextSeed.SeedAsync(movieContext);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

