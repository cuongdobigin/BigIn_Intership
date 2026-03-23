

using Microsoft.EntityFrameworkCore;
using MyApp.Application.Interface.Repository;
using webApi.Infrastructure.Persistence;

namespace webApi.Repository;

public class AccountRepository(AppDbContext context) : IAccountRepository
{
    public async Task<Account> FindByUsernameAsync(string username)
    {
        return await context.Accounts
            .Include(account => account.User)
            .FirstOrDefaultAsync(a => a.username == username) ;
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

    public async Task<Account> findById(int accountId)
    {
        return await context.Accounts.FindAsync(accountId);
        
    }
    
}