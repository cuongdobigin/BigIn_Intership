using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using MyApp.Application.Dto.Request;
using MyApp.Application.Dto.Response;
using MyApp.Application.Interface.Repository;
using MyApp.Application.Interface.Service;
using Myapp.Domain.Entity;
using Myapp.Domain.Exceptions;

namespace MyApp.Application.Service;

public class ShoppingCartService(
    IShoppingCartRepository cartRepository,
    IBookRepository bookRepository,
    IAccountRepository accountRepository,
    IMapper mapper,
    IHttpContextAccessor httpContextAccessor) : IShoppingCartService
{
    private string GetUsername()
    {
        return httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Name)?.Value
               ?? throw new AppException(ErrorCode.UNAUTHORIZED);
    }

    public async Task<ShoppingCartResponse> AddToCart(ShoppingCartRequest request)
    {
        var username = GetUsername();

        var account = await accountRepository.FindByUsernameAsync(username)
                      ?? throw new AppException(ErrorCode.ACCOUNT_NOT_FOUND);

        var book = await bookRepository.findById(request.BookId)
                   ?? throw new AppException(ErrorCode.BOOK_NOT_FOUND);
        if (book.quantity < request.Amount)
            throw new AppException(ErrorCode.INSUFFICIENT_QUANTITY);
        
        var existing = await cartRepository.GetByAccountUsername(username);
        if (existing.Any(c => c.Books.Id == request.BookId && c.isActive))
            throw new AppException(ErrorCode.CART_ITEM_ALREADY_EXISTS);

        var cartItem = new ShoppingCart
        {
            Amount = request.Amount,
            isActive = true,
            Account = account,
            Books = book
        };

        await cartRepository.Add(cartItem);
        return mapper.Map<ShoppingCartResponse>(cartItem);
    }

    public async Task<ShoppingCartResponse> UpdateCartItem(int id, ShoppingCartRequest request)
    {
        var username = GetUsername();

        var cartItem = await cartRepository.GetById(id)
                       ?? throw new AppException(ErrorCode.CART_NOT_FOUND);

        if (cartItem.Account.username != username)
            throw new AppException(ErrorCode.UNAUTHORIZED);
        

        var book = await bookRepository.findById(request.BookId)
                   ?? throw new AppException(ErrorCode.BOOK_NOT_FOUND);
        if (book.quantity < request.Amount)
            throw new AppException(ErrorCode.INSUFFICIENT_QUANTITY);

        cartItem.Amount = request.Amount;
        cartItem.Books = book;

        await cartRepository.Update(cartItem);
        return mapper.Map<ShoppingCartResponse>(cartItem);
    }

    public async Task RemoveFromCart(int id)
    {
        var username = GetUsername();

        var cartItem = await cartRepository.GetById(id)
                       ?? throw new AppException(ErrorCode.CART_NOT_FOUND);

        if (cartItem.Account.username != username)
            throw new AppException(ErrorCode.UNAUTHORIZED);

        await cartRepository.Delete(cartItem);
    }

    public async Task<List<ShoppingCartResponse>> GetMyCart()
    {
        var username = GetUsername();
        var items = await cartRepository.GetByAccountUsername(username);
        return mapper.Map<List<ShoppingCartResponse>>(items);
    }
}
