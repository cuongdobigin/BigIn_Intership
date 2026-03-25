using MyApp.Application.Dto.Request;
using MyApp.Application.Dto.Response;

namespace MyApp.Application.Interface.Service;

public interface IUserService
{
    Task<UserResponse> createUser(UserRequest request);
    Task<List<UserResponse>> findAll();
    Task<UserResponse> findById(int userId);
    Task<UserResponse> updateUser(UserRequest request);
}