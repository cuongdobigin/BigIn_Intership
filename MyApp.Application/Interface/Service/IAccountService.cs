using MyApp.Application.Dto.Request;
using MyApp.Application.Dto.Response;

namespace MyApp.Application.Interface.Service;

public interface IAccountService
{
    Task createAccount(RegisterRequest registerRequest);
    Task changePassword(ChangePasswordRequest request);
    
    Task deleteAccount(int accountId);
    Task<PageResponse<AccountResponse>> getAll(PageRequest pageRequest);
    Task UpdateAccount(int id, updateAcount updateAcount);
}