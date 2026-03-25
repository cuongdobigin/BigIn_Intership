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

public class ReviewService(
    IReviewRepository reviewRepository,
    IBookRepository bookRepository,
    IAccountRepository accountRepository,
    IMapper mapper,
    IHttpContextAccessor httpContextAccessor) : IReviewService
{
    
    public async Task<ReviewResponse> CreateReview(ReviewRequest request, int bookId)
    {
        var book = await bookRepository.findById(bookId)
                   ?? throw new AppException(ErrorCode.BOOK_NOT_FOUND);

        var account = await accountRepository.FindByUsernameAsync(getUsername())
                      ?? throw new AppException(ErrorCode.ACCOUNT_NOT_FOUND);

        var review = new Review
        {
            Message = request.Message,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            Book = book,
            Account = account
        };

        await reviewRepository.CreateReview(review);
        return mapper.Map<ReviewResponse>(review);
    }
    public string? getUsername()
    {
        return httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Name).Value;
    }
    public async Task<ReviewResponse> UpdateReview(int id, ReviewRequest request)
    {
        var review = await reviewRepository.GetReviewById(id)
                     ?? throw new AppException(ErrorCode.REVIEW_NOT_FOUND);

        if (review.Account.username != getUsername())
            throw new AppException(ErrorCode.UNAUTHORIZED);

        review.Message = request.Message;
        review.UpdatedAt = DateTime.UtcNow;

        await reviewRepository.UpdateReview(review);
        return mapper.Map<ReviewResponse>(review);
    }

    public async Task DeleteReview(int id)
    {
        var review = await reviewRepository.GetReviewById(id)
                     ?? throw new AppException(ErrorCode.REVIEW_NOT_FOUND);

        if (review.Account.username != getUsername())
            throw new AppException(ErrorCode.UNAUTHORIZED);

        await reviewRepository.DeleteReview(review);
    }

    public async Task<ReviewResponse> GetReviewById(int id)
    {
        var review = await reviewRepository.GetReviewById(id)
                     ?? throw new AppException(ErrorCode.REVIEW_NOT_FOUND);

        return mapper.Map<ReviewResponse>(review);
    }

    public async Task<PageResponse<ReviewResponse>> GetReviewsByBookId(int bookId, PageRequest pageRequest)
    {
        var (reviews, total) = await reviewRepository.GetReviewsByBookId(bookId, pageRequest.Page, pageRequest.PageSize);
        return new PageResponse<ReviewResponse>
        {
            Data = mapper.Map<List<ReviewResponse>>(reviews),
            Page = pageRequest.Page,
            PageSize = pageRequest.PageSize,
            TotalItems = total,
            TotalPages = (int)Math.Ceiling((double)total / pageRequest.PageSize)
        };
    }

    public async Task<PageResponse<ReviewResponse>> GetAllReviews(PageRequest pageRequest)
    {
        var (reviews, total) = await reviewRepository.GetAllReviews(pageRequest.Page, pageRequest.PageSize);
        return new PageResponse<ReviewResponse>
        {
            Data = mapper.Map<List<ReviewResponse>>(reviews),
            Page = pageRequest.Page,
            PageSize = pageRequest.PageSize,
            TotalItems = total,
            TotalPages = (int)Math.Ceiling((double)total / pageRequest.PageSize)
        };
    }
}
