using Microsoft.EntityFrameworkCore;
using webApi.Domain.Entity;
using webApi.Infrastructure.Persistence;
using webApi.Infrastructure.Persistence.Repository;

namespace webApi.Repository;

public class RoleRepository(AppDbContext appDbContext) : IRoleRepository
{
    public async Task<Role?> FindByNameAsync(string name)
    {
        return await appDbContext.Roles
            .FirstOrDefaultAsync(r => r.Name == name);
    }
    
    
}