using Myapp.Domain.Entity;

namespace MyApp.Application.Interface.Repository;

public interface IReviewRepository
{
    Task<Review> CreateReview(Review review);
    Task<Review> UpdateReview(Review review);
    Task DeleteReview(Review review);
    Task<Review?> GetReviewById(int id);
    Task<(List<Review>, int totalItems)> GetReviewsByBookId(int bookId, int page, int pageSize);
    Task<(List<Review>, int totalItems)> GetAllReviews(int page, int pageSize);
}
