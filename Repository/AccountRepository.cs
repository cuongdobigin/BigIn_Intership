using Microsoft.EntityFrameworkCore;
using webApi.Domain.Entity;
using webApi.Domain.Exceptions;
using webApi.Infrastructure.Persistence;
using webApi.Infrastructure.Persistence.Repository;

namespace webApi.Repository;

public class AccountRepository(AppDbContext context) : IAccountRepository
{
    public async Task<Account> FindByUsernameAsync(string username)
    {
        return await context.Accounts
            .FirstAsync(a => a.username == username);
    } 
}