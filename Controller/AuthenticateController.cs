using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyApp.Application.Dto.Request;
using MyApp.Application.Dto.Response;
using MyApp.Application.Interface.Service;


namespace webApi.Controllers;
[AllowAnonymous]
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
        var result = await _authenticationService.Login(loginRequest.Username, loginRequest.Password);
        
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true, // Set true if using HTTPS
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddSeconds(7 * 24 * 60 * 60) // Default 7 days, or read from config
        };
        Response.Cookies.Append("refreshToken", result.RefreshToken, cookieOptions);

        return ApiResponse<LoginResponse>.Success(new LoginResponse { token = result.AccessToken,isFirstTime = result.isFirstTime});
    }

    [HttpPost]
    [Route("refresh-token")]
    public async Task<ApiResponse<LoginResponse>> RefreshToken()
    {
        var refreshToken = Request.Cookies["refreshToken"];
        if (string.IsNullOrEmpty(refreshToken))
            throw new UnauthorizedAccessException("Refresh token not found");

        var newAccessToken = await _authenticationService.RefreshToken(refreshToken);
        return ApiResponse<LoginResponse>.Success(new LoginResponse { token = newAccessToken });
    }
}