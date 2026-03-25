using Myapp.Domain.Entity;

namespace MyApp.Application.Interface.Repository;

public interface IRoleRepository
{
    Task<Role> FindByNameAsync(string name);
}