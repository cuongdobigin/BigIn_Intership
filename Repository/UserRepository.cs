

using Microsoft.EntityFrameworkCore;
using webApi.Domain.Entity;
using webApi.Dto.Request;
using webApi.Infrastructure.Persistence;
using webApi.Infrastructure.Persistence.Repository;

namespace webApi.Repository;

public class UserRepository(AppDbContext context): IUserRepository
{
    public async Task<List<User>> findAll()
    {
        return await context.Users
            .Include(u=>u.Account)
            .Where(u=>u.Account!=null && u.Account.isActive==true)
            .ToListAsync();
    }

    public async Task<User?> findByIdAsync(int userId)
    {
        return await context.Users
            .Include(u=>u.Account)
            .Where(u=>u.Account!=null && u.Account.isActive==true)
            .FirstOrDefaultAsync(u => u.Id == userId);
    }

    public async Task<User> updateUser(User user)
    {
        context.Users.Update(user);
        await context.SaveChangesAsync();
        return user;
    }

    public async Task<User> createUser(User request)
    {

        context.Users.Add(request);
        await context.SaveChangesAsync();
        return request;
    }

    public async Task<User> findByAccount_Username(string username)
    {
        return await context.Users
            .Include(u=>u.Account)
            .Where(u=>u.Account.username == username)
            .FirstOrDefaultAsync();
    }
}