using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using StockMarket.Data.IoC;
using StockMarket.Domain.Context;
using StockMarket.Service.IoC;
using StockMarket.Service.Settings;
using System.Net.Http.Headers;
using System.Text;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using System;

var builder = WebApplication.CreateBuilder(args);

//Add services to the container.
var keyVaultUrl = builder.Configuration.GetSection("KeyVaultUrl").Get<string>();

var client = new SecretClient(new Uri(keyVaultUrl), new DefaultAzureCredential());
var connectionString = client.GetSecret("ConnectionStrings").Value.Value;
var hashSecret = client.GetSecret("HashSecret").Value.Value;
var stockDataApiKey = client.GetSecret("StockDataApiKey").Value.Value;

builder.Services.AddDbContext<StockMarketContext>(options =>
    options.UseSqlServer(connectionString)
                .UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole())));
builder.Services.AddScoped<DbContext, StockMarketContext>();

var stockDataApiUrl = builder.Configuration.GetSection("StockDataApiUrl").Get<string>();
builder.Services.AddHttpClient("StockData", c =>
{
    c.BaseAddress = new Uri(stockDataApiUrl);
    c.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", stockDataApiKey);
});

RepositoriesInjector.RegisterRepositories(builder.Services);
ServicesInjector.RegisterServices(builder.Services);


var key = Encoding.ASCII.GetBytes(hashSecret);
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "StockMarket.Api", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "Bearer",
                            Name = "Bearer",
                            In = ParameterLocation.Header
                        },
                        new List<string>()
                    }
                });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "MY_CORS",
                      policy =>
                      {
                          policy.AllowAnyOrigin();
                          policy.AllowAnyMethod();
                          policy.AllowAnyHeader();
                      });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

}
app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "StockMarket.Api v1"));

app.UseCors("MY_CORS");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
