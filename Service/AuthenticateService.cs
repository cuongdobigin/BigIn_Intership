using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using webApi.Domain.Exceptions;
using webApi.Dto.Response;
using webApi.Infrastructure.Persistence.Repository;
using webApi.Service.Interface;

namespace webApi.Service;

public class AuthenticateService(IAccountRepository accountRepository,PasswordService passwordService,IConfiguration config) : IAuthenticateService
{
    private readonly IAccountRepository _accountRepository=accountRepository;
    private readonly PasswordService _passwordService=passwordService;
    private readonly IConfiguration _config=config;          
    

    public async Task<LoginResult> Login(string username, string password)
    {
        var account = await _accountRepository.FindByUsernameAsync(username);
        if (account is null)
            throw new AppException(ErrorCode.ACCOUNT_NOT_FOUND);

        if (!_passwordService.VerifyPassword(password, account.password))  
            throw new AppException(ErrorCode.PASSWORD_ERROR);
        
        return new LoginResult
        {
            AccessToken = GenerateToken(account.username),
            RefreshToken = GenerateRefreshToken(account.username)
        };
    }

    public async Task<string> RefreshToken(string refreshToken)
    {
        var key = _config["Jwt:SecretKeyRefresh"]
                  ?? throw new AppException(ErrorCode.SECRETKEY_NOT_FOUND);

        var tokenHandler = new JwtSecurityTokenHandler();
        try
        {
            var principal = tokenHandler.ValidateToken(refreshToken, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                ValidateIssuer = true,   
                ValidIssuers = new[] { "http://localhost:5052" },
    
                ValidateAudience = true,
                ValidAudiences = new[] { "http://localhost:5052" },
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var username = principal.Identity.Name;
            return GenerateToken(username);
        }
        catch
        {
            throw new AppException(ErrorCode.REFRESH_TOKEN_INVALID); 
        }
    }

    private string GenerateToken(string username)
    {
        var key = _config["Jwt:SecretKey"]
                  ?? throw new AppException(ErrorCode.SECRETKEY_NOT_FOUND);

        var expireSeconds = int.Parse(_config["Jwt:ExpireSeconds"] 
                                      ?? throw new AppException(ErrorCode.EXPIRESECONDS_NOT_FOUND));
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, username),
            new Claim("TokenType", "accessToken")
        };

        var token = new JwtSecurityToken(
            issuer:"http://localhost:5052",
            audience:"http://localhost:5052",
            claims: claims,
            expires: DateTime.UtcNow.AddSeconds(expireSeconds),     
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                SecurityAlgorithms.HmacSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private string GenerateRefreshToken(string username)
    {
        var key = _config["Jwt:SecretKeyRefresh"]
                  ?? throw new AppException(ErrorCode.SECRETKEY_NOT_FOUND);

        var expireSeconds = int.Parse(_config["Jwt:RefreshTokenExpireSeconds"] 
                                      ?? throw new AppException(ErrorCode.EXPIRESECONDS_NOT_FOUND));
        
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, username),
            new Claim("TokenType", "RefreshToken")
        };

        var token = new JwtSecurityToken(
            issuer:"http://localhost:5052",
            audience:"http://localhost:5052",
            claims: claims,
            expires: DateTime.UtcNow.AddSeconds(expireSeconds),
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                SecurityAlgorithms.HmacSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}