using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using webApi.configuration;
using webApi.Infrastructure.Persistence;
using webApi.Infrastructure.Persistence.Repository;
using webApi.Infrastructure.Middleware;
using webApi.Repository;
using webApi.Service;
using webApi.Service.Interface;



var builder = WebApplication.CreateBuilder(args);
var key = "THIS_IS_SECRET_KEY_123456"; // nên để trong config

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false, 
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(key))
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));

builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<PasswordService>();
builder.Services.AddScoped<IAuthenticateService, AuthenticateService>();
var app = builder.Build();

await ApplicationInitConfig.InitializeAsync(app.Services);


app.UseMiddleware<ExceptionHandlingMiddleware>(); 
app.UseHttpsRedirection();
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication(); 
app.UseAuthorization();

app.MapControllers();

app.Run();