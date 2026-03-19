using webApi.Domain.Entity;
using webApi.Dto.Request;
using webApi.Dto.Response;

namespace webApi.Service.Interface;

public interface IUserService
{
    Task<UserResponse> createUser(CreateUserRequest request);
    Task<List<UserResponse>> findAll();
    Task<UserResponse> findById(int userId);
    Task<UserResponse> updateUser(CreateUserRequest request);
}