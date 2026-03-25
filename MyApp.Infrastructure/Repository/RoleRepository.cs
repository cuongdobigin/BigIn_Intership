

using Microsoft.EntityFrameworkCore;
using MyApp.Application.Interface.Repository;
using Myapp.Domain.Entity;
using webApi.Infrastructure.Persistence;

namespace webApi.Repository;

public class RoleRepository(AppDbContext appDbContext) : IRoleRepository
{
    public async Task<Role?> FindByNameAsync(string name)
    {
        return await appDbContext.Roles
            .FirstOrDefaultAsync(r => r.Name == name);
    }
    
    
}