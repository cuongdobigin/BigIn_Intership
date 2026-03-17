using webApi.Domain.Entity;

namespace webApi.Infrastructure.Persistence.Repository;

public interface IAccountRepository
{
    Task<Account> FindByUsernameAsync(string username);
}