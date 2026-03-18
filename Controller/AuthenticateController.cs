using Microsoft.AspNetCore.Authentication;

using Microsoft.AspNetCore.Mvc;
using webApi.Dto.Request;
using webApi.Dto.Response;
using webApi.Service.Interface;

namespace webApi.Controllers;
[ApiController]
[Route("api/auth")]
public class AuthenticateController : ControllerBase
{
    private readonly IAuthenticateService _authenticationService;
    public AuthenticateController(IAuthenticateService authenticationService)
    {
        _authenticationService = authenticationService;
    }
    [HttpPost]
    [Route("login")]
    public async Task<ApiResponse<LoginResponse>> Login([FromBody] LoginRequest loginRequest)
    {
        var result= await _authenticationService.Login(loginRequest.Username, loginRequest.Password);
        return ApiResponse<LoginResponse>.Success(result);
    }
}