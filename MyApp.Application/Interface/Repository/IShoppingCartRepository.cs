using Myapp.Domain.Entity;

namespace MyApp.Application.Interface.Repository;

public interface IShoppingCartRepository
{
    Task<ShoppingCart> Add(ShoppingCart cart);
    Task<ShoppingCart> Update(ShoppingCart cart);
    Task Delete(ShoppingCart cart);
    Task<ShoppingCart?> GetById(int id);
    Task<List<ShoppingCart>> GetByAccountUsername(string username);
}
