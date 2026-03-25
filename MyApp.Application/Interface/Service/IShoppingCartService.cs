using MyApp.Application.Dto.Request;
using MyApp.Application.Dto.Response;

namespace MyApp.Application.Interface.Service;

public interface IShoppingCartService
{
    Task<ShoppingCartResponse> AddToCart(ShoppingCartRequest request);
    Task<ShoppingCartResponse> UpdateCartItem(int id, ShoppingCartRequest request);
    Task RemoveFromCart(int id);
    Task<List<ShoppingCartResponse>> GetMyCart();
}
