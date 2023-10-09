using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using System.Text;
using TicketingSystem;
using TicketingSystem.Model;
using TicketingSystem.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors();
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<DatabaseSetting>(builder.Configuration.GetSection(nameof(DatabaseSetting)));

builder.Services.AddSingleton<DatabaseSetting>(sp => sp.GetRequiredService<IOptions<DatabaseSetting>>().Value);
builder.Services.AddSingleton<IMongoClient>(s => new MongoClient(builder.Configuration.GetValue<string>("DatabaseSetting:ConnectionString")));

builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<LoginService>();
builder.Services.AddScoped<ReservationService>();
builder.Services.AddScoped<TrainService>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "iis_asdasdasdasdasfdasfas", 
            ValidAudience = "abcd_adsdasdasdasdasdsad",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("YourSecretKey_sfdsfewrdgdfhthhfghdh")), // Replace with your secret key
        };
    });

builder.Services.AddSingleton<JwtTokenService>(provider =>
{
    // Configure and create an instance of JwtTokenService here
    // You should provide the necessary parameters for JwtTokenService
    // For example, pass your secret key, issuer, audience, and token expiry.
    return new JwtTokenService("YourSecretKey_sfdsfewrdgdfhthhfghdh", "iis_asdasdasdasdasfdasfas", "abcd_adsdasdasdasdasdsad", 12000);
});


var app = builder.Build();
//app.UseCors(builder => builder.AllowAnyHeader().AllowAnyOrigin()
//                                        .AllowAnyMethod()
//                                        .AllowCredentials());
//
app.UseCors(builder => builder
    .WithOrigins("http://localhost:3000", "https://another.com")
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
