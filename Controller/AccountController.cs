using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyApp.Application.Dto.Request;
using MyApp.Application.Dto.Response;
using MyApp.Application.Interface.Service;


namespace webApi.Controllers;
[Authorize]
[ApiController]
[Route("api/account")]
public class AccountController(IAccountService accountService): ControllerBase
{
    [AllowAnonymous]
    [HttpPost]
    public async Task<ApiResponse<string>> createAccount([FromBody]RegisterRequest registerRequest)
    {
        await accountService.createAccount(registerRequest);
        return ApiResponse<string>.Success();
    }
    [HttpPost("change-password")]
    public async Task<ApiResponse<string>> changePassword([FromBody]ChangePasswordRequest request)
    {
        await accountService.changePassword(request);
        return ApiResponse<string>.Success();
    }

    [HttpDelete("{accountId}")]
    public async Task<ApiResponse<string>> deleteAccount(int accountId)
    {
        await accountService.deleteAccount(accountId);
        return ApiResponse<string>.Success();
    }
}