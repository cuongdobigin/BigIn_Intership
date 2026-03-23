using Microsoft.EntityFrameworkCore;
using MyApp.Application.Interface.Repository;
using Myapp.Domain.Entity;
using webApi.Infrastructure.Persistence;

namespace webApi.Repository;

public class ReviewRepository(AppDbContext context) : IReviewRepository
{
    public async Task<Review> CreateReview(Review review)
    {
        await context.Reviews.AddAsync(review);
        await context.SaveChangesAsync();
        return review;
    }

    public async Task<Review> UpdateReview(Review review)
    {
        context.Reviews.Update(review);
        await context.SaveChangesAsync();
        return review;
    }

    public async Task DeleteReview(Review review)
    {
        context.Reviews.Remove(review);
        await context.SaveChangesAsync();
    }

    public async Task<Review?> GetReviewById(int id)
    {
        return await context.Reviews
            .Include(r => r.Account)
            .Include(r => r.Book)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<(List<Review>, int totalItems)> GetReviewsByBookId(int bookId, int page, int pageSize)
    {
        var query = context.Reviews
            .Include(r => r.Account)
            .Where(r => r.Book.Id == bookId && r.IsActive);
        int total = await query.CountAsync();
        var list = await query
            .OrderByDescending(r => r.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        return (list, total);
    }

    public async Task<(List<Review>, int totalItems)> GetAllReviews(int page, int pageSize)
    {
        var query = context.Reviews
            .Include(r => r.Account)
            .Include(r => r.Book);
        int total = await query.CountAsync();
        var list = await query
            .OrderByDescending(r => r.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        return (list, total);
    }
}
