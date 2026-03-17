using Microsoft.AspNetCore.Authentication;

using Microsoft.AspNetCore.Mvc;
using webApi.Dto.Request;
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
    public async Task<IActionResult> Login([FromBody] RegisterRequest registerRequest)
    {
        return Ok(await _authenticationService.Login(registerRequest.Username, registerRequest.Password));
    }
}