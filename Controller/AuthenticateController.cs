using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
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
        return Ok(_authenticationService.Login(registerRequest.Email, registerRequest.Password));
    }
}