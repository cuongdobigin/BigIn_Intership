using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using MyApp.Application.Dto.Request;
using MyApp.Application.Dto.Response;
using MyApp.Application.Interface.Repository;
using MyApp.Application.Interface.Service;
using Myapp.Domain.Entity;
using Myapp.Domain.Exceptions;
using System.Security.Claims;
namespace MyApp.Application.Service;
public class UserService(IUserRepository userRepository,IAccountRepository accountRepository,IMapper mapper,IHttpContextAccessor httpContextAccessor) :IUserService
{


    public async Task<List<UserResponse>> findAll()
    {
        var users=await userRepository.findAll();
        return mapper.Map<List<UserResponse>>(users);
    }

    public async Task<UserResponse> findById(int userId)
    {
        var user= await userRepository.findByIdAsync(userId)
            ?? throw new AppException(ErrorCode.USER_NOT_FOUND);
        return mapper.Map<UserResponse>(user);
    }

    public async Task<UserResponse> createUser(UserRequest request)
    {
        var username = getUsername() ?? throw new AppException(ErrorCode.UNAUTHORIZED);
        Account account = await accountRepository.FindByUsernameAsync(username)
            ?? throw new AppException(ErrorCode.ACCOUNT_NOT_FOUND);
        if (account.User != null)
            throw new AppException(ErrorCode.USER_EXISTED);
        var userRequet =mapper.Map<User>(request);
        userRequet.Account = account;
        var user=await userRepository.createUser(userRequet);
        account.isFirstTime = false;
        await accountRepository.UpdateAsync(account);
        var userResponse= mapper.Map<UserResponse>(user);
        userResponse.usernameAccount = account.username;
        return userResponse;
    }

    public string? getUsername()
    {
        return httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Name).Value;
    }

    public async Task<UserResponse> updateUser(UserRequest request)
    {
        var user = await userRepository.findByAccount_Username(getUsername())
                   ?? throw new AppException(ErrorCode.USER_NOT_FOUND);
        mapper.Map(request, user);
        var updated = await userRepository.updateUser(user);
        return mapper.Map<UserResponse>(updated);
    }
}