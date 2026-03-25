using MyApp.Application.Dto.Request;

namespace MyApp.Application.Interface.Service;

public interface IAccountService
{
    Task createAccount(RegisterRequest registerRequest);
    Task changePassword(ChangePasswordRequest request);
    
    Task deleteAccount(int accountId);
}