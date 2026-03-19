using webApi.Domain.Entity;

namespace webApi.Infrastructure.Persistence.Repository;

public interface IRoleRepository
{
    Task<Role> FindByNameAsync(string name);
}