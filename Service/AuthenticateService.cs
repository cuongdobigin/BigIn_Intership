using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using webApi.Domain.Exceptions;
using webApi.Infrastructure.Persistence.Repository;
using webApi.Service.Interface;

namespace webApi.Service;

public class AuthenticateService : IAuthenticateService
{
    private readonly IAccountRepository _accountRepository;
    private readonly PasswordService _passwordService;
    private readonly IConfiguration _config;          

    public AuthenticateService(
        IAccountRepository accountRepository,
        PasswordService passwordService,
        IConfiguration config)                        
    {
        _accountRepository = accountRepository;
        _passwordService = passwordService;
        _config = config;                            
    }

    public async Task<string> Login(string username, string password)
    {
        var account = await _accountRepository.FindByUsernameAsync(username);
        if (account is null)
            throw new AppException(ErrorCode.ACCOUNT_NOT_FOUND);

        if (!_passwordService.VerifyPassword(password, account.password))  
            throw new AppException(ErrorCode.PASSWORD_ERROR);

        return GenerateToken(account.username);
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
            new Claim(ClaimTypes.Role, "Admin")
        };

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddSeconds(expireSeconds),     
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                SecurityAlgorithms.HmacSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}