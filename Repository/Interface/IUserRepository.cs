using webApi.Domain.Entity;
using webApi.Dto.Request;

namespace webApi.Infrastructure.Persistence.Repository;

public interface IUserRepository
{
    Task<User> findByIdAsync(int userId);
    Task<User> createUser(User request);
    Task<List<User>> findAll();
    Task<User> updateUser(User user);
    Task<User> findByAccount_Username(string username);
}