using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyApp.Application.Dto.Request;
using MyApp.Application.Dto.Response;
using MyApp.Application.Interface.Service;


namespace webApi.Controller;

[ApiController]
[Route("api/users")]
[Authorize]
public class UserController(IUserService userService) : ControllerBase
{   
    [HttpPost]
    public async Task<ApiResponse<UserResponse>> createUser ([FromBody]UserRequest request)
    {
        return ApiResponse<UserResponse>.Success(await userService.createUser(request));
    }
    [HttpPut]
    public async Task<ApiResponse<UserResponse>> updateUser([FromBody]UserRequest request)
    {
        return ApiResponse<UserResponse>.Success(await userService.updateUser(request));
    }

    [HttpGet("{userId}")]
    public async Task<ApiResponse<UserResponse>> findUserById(int userId)
    {
        return ApiResponse<UserResponse>.Success(await userService.findById(userId));
    }

    [HttpGet]
    public async Task<ApiResponse<List<UserResponse>>> findAllUsers()
    {
        return ApiResponse<List<UserResponse>>.Success(await userService.findAll());
    }
    
    
}