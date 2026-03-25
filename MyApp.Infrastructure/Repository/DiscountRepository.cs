using Microsoft.EntityFrameworkCore;
using MyApp.Application.Interface.Repository;
using Myapp.Domain.Entity;
using webApi.Infrastructure.Persistence;

namespace webApi.Repository;

public class DiscountRepository(AppDbContext context) : IDiscountRepository
{
    public async Task<Discount> Create(Discount discount)
    {
        await context.Discounts.AddAsync(discount);
        await context.SaveChangesAsync();
        return discount;
    }

    public async Task<Discount> Update(Discount discount)
    {
        context.Discounts.Update(discount);
        await context.SaveChangesAsync();
        return discount;
    }

    public async Task Delete(Discount discount)
    {
        context.Discounts.Remove(discount);
        await context.SaveChangesAsync();
    }

    public async Task<Discount?> GetById(int id)
    {
        return await context.Discounts
            .FirstOrDefaultAsync(d => d.Id == id && d.IsActive);
    }

    public async Task<List<Discount>> GetAll()
    {
        return await context.Discounts
            .OrderByDescending(d => d.Percent)
            .Where(d => d.IsActive)
            .ToListAsync();
    }

    public async Task<bool> ExistsByPercent(decimal percent,DateTime startDate, DateTime endDate)
    {
        return await context.Discounts
            .Where(d=>d.StartDate== startDate && d.EndDate== endDate)
            .AnyAsync(d => d.Percent == percent && d.IsActive);
    }
}
