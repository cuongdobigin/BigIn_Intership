using webApi.Dto.Response;

namespace webApi.Service.Interface;

public interface IAuthenticateService
{
    Task<LoginResult> Login(string username, string password);
    Task<string> RefreshToken(string refreshToken);
}