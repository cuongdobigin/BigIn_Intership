using Microsoft.EntityFrameworkCore;
using MyApp.Application.Interface.Repository;
using Myapp.Domain.Entity;
using webApi.Infrastructure.Persistence;

namespace webApi.Repository;

public class ShoppingCartRepository(AppDbContext context) : IShoppingCartRepository
{
    public async Task<ShoppingCart> Add(ShoppingCart cart)
    {
        await context.ShoppingCarts.AddAsync(cart);
        await context.SaveChangesAsync();
        return cart;
    }

    public async Task<ShoppingCart> Update(ShoppingCart cart)
    {
        context.ShoppingCarts.Update(cart);
        await context.SaveChangesAsync();
        return cart;
    }

    public async Task Delete(ShoppingCart cart)
    {
        context.ShoppingCarts.Remove(cart);
        await context.SaveChangesAsync();
    }

    public async Task<ShoppingCart?> GetById(int id)
    {
        return await context.ShoppingCarts
            .Include(c => c.Account)
            .Include(c => c.Books)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<List<ShoppingCart>> GetByAccountUsername(string username)
    {
        return await context.ShoppingCarts
            .Include(c => c.Account)
            .Include(c => c.Books)
            .Where(c => c.Account.username == username && c.isActive)
            .ToListAsync();
    }
}
