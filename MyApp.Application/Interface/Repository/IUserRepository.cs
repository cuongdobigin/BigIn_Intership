using Myapp.Domain.Entity;

namespace MyApp.Application.Interface.Repository;

public interface IUserRepository
{
    Task<User> findByIdAsync(int userId);
    Task<User> createUser(User request);
    Task<List<User>> findAll();
    Task<User> updateUser(User user);
    Task<User> findByAccount_Username(string username);
}