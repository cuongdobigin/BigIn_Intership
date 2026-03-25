

using AutoMapper;
using MyApp.Application.Dto.Request;
using MyApp.Application.Interface.Repository;
using MyApp.Application.Interface.Service;
using Myapp.Domain.Exceptions;
using webApi.Service;

namespace MyApp.Application.Service;

public class AccountService(IAccountRepository accountRepository,IMapper mapper,PasswordService passwordService,IRoleRepository roleRepository) : IAccountService
{
    private IAccountRepository _accountRepository=accountRepository;
    private IMapper _mapper=mapper;
    private PasswordService _passwordService=passwordService;
    private  IRoleRepository _roleRepository=roleRepository;
    

    public async Task createAccount(RegisterRequest registerRequest)
    {  
        if( await _accountRepository.FindByUsernameAsync(registerRequest.username) != null)
            throw new AppException(ErrorCode.EMAIL_EXISTED);
        var account = _mapper.Map<Account>(registerRequest);
        account.password = _passwordService.HashPassword(registerRequest.password);
        var role = await _roleRepository.FindByNameAsync("user");
        if (role != null)
            account.Roles.Add(role);
        await accountRepository.AddAsync(account);
    }

    public async Task changePassword(ChangePasswordRequest request)
    {
        var account = await _accountRepository.FindByUsernameAsync(request.username);
        if (account == null)
            throw new AppException(ErrorCode.ACCOUNT_NOT_FOUND);

        if (!_passwordService.VerifyPassword(request.oldPassword, account.password))
            throw new AppException(ErrorCode.OLD_PASSWORD_ERROR);
        
        account.password = _passwordService.HashPassword(request.newPassword);
        await _accountRepository.UpdateAsync(account);
    }

    public async Task deleteAccount(int accountId)
    {
        var account=await _accountRepository.findById(accountId);
        if (account == null)
            throw new AppException(ErrorCode.ACCOUNT_NOT_FOUND);
        account.isActive = false;
        await _accountRepository.UpdateAsync(account);
    }
}