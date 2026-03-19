using webApi.Domain.Entity;

namespace webApi.Infrastructure.Persistence.Repository;

public interface IAccountRepository
{
    Task<Account> FindByUsernameAsync(string username);
    Task<Account> AddAsync(Account account);
    Task UpdateAsync(Account account);
    Task<Account> findById(int accountId);
}