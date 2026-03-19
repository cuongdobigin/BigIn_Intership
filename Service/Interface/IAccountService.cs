using webApi.Dto.Request;

namespace webApi.Service.Interface;

public interface IAccountService
{
    Task createAccount(RegisterRequest registerRequest);
    Task changePassword(ChangePasswordRequest request);
    
    Task deleteAccount(int accountId);
}