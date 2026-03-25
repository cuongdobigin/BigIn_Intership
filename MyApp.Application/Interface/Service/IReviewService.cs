using Microsoft.AspNetCore.Http;
using MyApp.Application.Dto.Request;
using MyApp.Application.Dto.Response;

namespace MyApp.Application.Interface.Service;

public interface IReviewService
{
    Task<ReviewResponse> CreateReview(ReviewRequest request, int bookId);
    Task<ReviewResponse> UpdateReview(int id, ReviewRequest request);
    Task DeleteReview(int id);
    Task<ReviewResponse> GetReviewById(int id);
    Task<PageResponse<ReviewResponse>> GetReviewsByBookId(int bookId, PageRequest pageRequest);
    Task<PageResponse<ReviewResponse>> GetAllReviews(PageRequest pageRequest);
}
