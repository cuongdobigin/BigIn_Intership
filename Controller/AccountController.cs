using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using webApi.Domain.Entity;
using webApi.Dto.Request;
using webApi.Dto.Response;
using webApi.Service.Interface;

namespace webApi.Controllers;
[Authorize]
[ApiController]
[Route("api/account")]
public class AccountController(IAccountService accountService): ControllerBase
{
    private readonly IAccountService _accountService= accountService;
    [AllowAnonymous]
    [HttpPost]
    public async Task<ApiResponse<string>> createAccount([FromBody]RegisterRequest registerRequest)
    {
        await _accountService.createAccount(registerRequest);
        return ApiResponse<string>.Success();
    }
    [HttpPost("change-password")]
    public async Task<ApiResponse<string>> changePassword([FromBody]ChangePasswordRequest request)
    {
        await _accountService.changePassword(request);
        return ApiResponse<string>.Success();
    }
}