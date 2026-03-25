using Microsoft.EntityFrameworkCore;
using MyApp.Application.Interface.Repository;
using Myapp.Domain.Entity;
using webApi.Infrastructure.Persistence;

namespace webApi.Repository;

public class OrderRepository(AppDbContext context) : IOrderRepository
{
    public async Task<Order> Create(Order order)
    {
        await context.Orders.AddAsync(order);
        await context.SaveChangesAsync();
        return order;
    }

    public async Task<Order> Update(Order order)
    {
        context.Orders.Update(order);
        await context.SaveChangesAsync();
        return order;
    }

    public async Task<Order?> GetById(int id)
    {
        return await context.Orders
            .Include(o => o.Account)
            .Include(o => o.Discount)
            .Include(o => o.ShoppingCarts)
                .ThenInclude(s => s.Books)
            .FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task<List<Order>> GetMyOrders(string username)
    {
        return await context.Orders
            .Include(o => o.Discount)
            .Include(o => o.ShoppingCarts)
                .ThenInclude(s => s.Books)
            .Where(o => o.Account.username == username && o.IsActive)
            .OrderByDescending(o => o.Id)
            .ToListAsync();
    }
}
