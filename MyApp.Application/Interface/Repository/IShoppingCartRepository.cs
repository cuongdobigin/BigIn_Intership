using Myapp.Domain.Entity;

namespace MyApp.Application.Interface.Repository;

public interface IShoppingCartRepository
{
    Task<List<ShoppingCart>> GetByAccountUsername(string username);
    Task<ShoppingCart?> GetById(int id);
    Task<ShoppingCart> Add(ShoppingCart cartItem);
    Task<ShoppingCart> Update(ShoppingCart cartItem);
    Task Delete(ShoppingCart cartItem);
}
