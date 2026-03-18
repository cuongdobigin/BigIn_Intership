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
            .FirstOrDefaultAsync(a => a.username == username);
    }

    public async Task<Account> AddAsync(Account account)
    {
        context.Accounts.Add(account);
        await context.SaveChangesAsync();
        return account;
    }

    public async Task UpdateAsync(Account account)
    {
        context.Accounts.Update(account);
        await context.SaveChangesAsync();
    }
}