using webApi.Dto.Response;

namespace webApi.Service.Interface;

public interface IAuthenticateService
{
    Task<LoginResponse> Login(string username, string password);
}