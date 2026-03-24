using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyApp.Application.Dto.Request;
using MyApp.Application.Dto.Response;
using MyApp.Application.Interface.Service;

namespace webApi.Controller;

[Authorize]
[ApiController]
[Route("api/cart")]
public class ShoppingCartController(IShoppingCartService cartService) : ControllerBase
{

    [HttpPost]
    public async Task<ApiResponse<ShoppingCartResponse>> AddToCart([FromBody] ShoppingCartRequest request)
    {
        var result = await cartService.AddToCart(request);
        return ApiResponse<ShoppingCartResponse>.Success(result);
    }
    
    [HttpGet]
    public async Task<ApiResponse<List<ShoppingCartResponse>>> GetMyCart()
    {
        var result = await cartService.GetMyCart();
        return ApiResponse<List<ShoppingCartResponse>>.Success(result);
    }


    [HttpPut("{id}")]
    public async Task<ApiResponse<ShoppingCartResponse>> UpdateCartItem(int id, [FromBody] ShoppingCartRequest request)
    {
        var result = await cartService.UpdateCartItem(id, request);
        return ApiResponse<ShoppingCartResponse>.Success(result);
    }


    [HttpDelete("{id}")]
    public async Task<ApiResponse<string>> RemoveFromCart(int id)
    {
        await cartService.RemoveFromCart(id);
        return ApiResponse<string>.Success();
    }
}
