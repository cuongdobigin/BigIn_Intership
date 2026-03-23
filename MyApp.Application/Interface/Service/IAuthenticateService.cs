using MyApp.Application.Dto.Response;

namespace MyApp.Application.Interface.Service;
public interface IAuthenticateService
{
    Task<LoginResult> Login(string username, string password);
    Task<string> RefreshToken(string refreshToken);
}