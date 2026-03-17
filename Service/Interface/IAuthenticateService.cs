namespace webApi.Service.Interface;

public interface IAuthenticateService
{
    Task<string> Login(string username, string password);
}