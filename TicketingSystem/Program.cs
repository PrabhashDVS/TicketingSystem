/*
   File: Program.cs
   Description: This file contains the configuration of the ASP.NET Core application, including service registration,
   CORS settings, authentication, and request pipeline setup.
   Author: Prabhash D.V.S. , Piyumantha H. P. A. H. , Weerasinghe T. K. , Weerasiri R. T. K.
   Creation Date: 2023/10/02
   Last Modified Date: 2023/10/12
*/

using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using System.Text;
using TicketingSystem;
using TicketingSystem.Model;
using TicketingSystem.Model.ViewModels;
using TicketingSystem.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<DatabaseSetting>(builder.Configuration.GetSection(nameof(DatabaseSetting)));
builder.Services.AddSingleton<DatabaseSetting>(sp => sp.GetRequiredService<IOptions<DatabaseSetting>>().Value);
builder.Services.AddSingleton<IMongoClient>(s => new MongoClient(builder.Configuration.GetValue<string>("DatabaseSetting:ConnectionString")));
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<LoginService>();
builder.Services.AddScoped<ReservationService>();
builder.Services.AddScoped<TrainService>();

var corsSettings = builder.Configuration.GetSection("CorsSettings").Get<CorsSettings>();
var jwtSettings = builder.Configuration.GetSection("JWTSettings").Get<JWTSettings>();

// add authorization for roles 
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("backOfficersOnly", policy => policy.RequireRole("backOfficer"));
});


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings.ValidIssuer,
            ValidAudience = jwtSettings.ValidAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),
        };
    });

builder.Services.AddSingleton<JwtTokenService>(provider =>
{
    return new JwtTokenService(jwtSettings.SecretKey, jwtSettings.ValidIssuer, jwtSettings.ValidAudience, 12000);
});

// config Mapper
var mapperConfiguration = new MapperConfiguration(cfg =>
{
    cfg.AddProfile<MappingProfile>();
});
var mapper = new Mapper(mapperConfiguration);

builder.Services.AddSingleton<IMapper>(mapper);

var app = builder.Build();
app.UseCors(builder => builder
    .WithOrigins(corsSettings.AllowedOrigins)
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowCredentials());

app.UseCors("CorsPolicy");
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
