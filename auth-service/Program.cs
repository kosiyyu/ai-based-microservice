using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Services;
using Settings;

var builder = WebApplication.CreateBuilder(args);

var jwtIssuer = EnvSettings.JwtIssuer;
var jwtKey = EnvSettings.JwtKey;

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(x => 
    {
        x.TokenValidationParameters = new TokenValidationParameters
        {
            // LogTokenId = true,
            // LogValidationExceptions = true,
            // RequireExpirationTime = true,
            // RequireSignedTokens = true,
            // RequireAudience = true,
            // SaveSigninToken = false,
            // TryAllIssuerSigningKeys = true,
            // ValidateActor = false,
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            // ValidateTokenReplay = false,

            ValidIssuer = jwtIssuer,
            ValidAudience = jwtIssuer,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

builder.Services.AddSingleton<MongoDBSettings>();
builder.Services.AddSingleton<UserService>();

builder.Services.AddControllers();

var app = builder.Build();
app.MapControllers();

app.Run();