using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyApp.Application.Dto.Request;
using MyApp.Application.Dto.Response;
using MyApp.Application.Interface.Service;
using Myapp.Domain.Exceptions;

namespace webApi.Controller;

[Authorize]
[ApiController]
[Route("api/reviews")]
public class ReviewController(IReviewService reviewService) : ControllerBase
{
    [HttpPost("{bookId}")]
    public async Task<ApiResponse<ReviewResponse>> CreateReview(int bookId, [FromBody] ReviewRequest request)
    {
        var username = User.Identity?.Name ?? throw new AppException(ErrorCode.UNAUTHORIZED);
        var result = await reviewService.CreateReview(request, bookId);
        return ApiResponse<ReviewResponse>.Success(result);
    }

    [HttpGet]
    public async Task<ApiResponse<PageResponse<ReviewResponse>>> GetAllReviews([FromQuery] PageRequest pageRequest)
    {
        var result = await reviewService.GetAllReviews(pageRequest);
        return ApiResponse<PageResponse<ReviewResponse>>.Success(result);
    }

    [HttpGet("book/{bookId}")]
    public async Task<ApiResponse<PageResponse<ReviewResponse>>> GetReviewsByBookId(int bookId, [FromQuery] PageRequest pageRequest)
    {
        var result = await reviewService.GetReviewsByBookId(bookId, pageRequest);
        return ApiResponse<PageResponse<ReviewResponse>>.Success(result);
    }

    [HttpGet("{id}")]
    public async Task<ApiResponse<ReviewResponse>> GetReviewById(int id)
    {
        var result = await reviewService.GetReviewById(id);
        return ApiResponse<ReviewResponse>.Success(result);
    }

    [HttpPut("{id}")]
    public async Task<ApiResponse<ReviewResponse>> UpdateReview(int id, [FromBody] ReviewRequest request)
    {
        var username = User.Identity?.Name ?? throw new AppException(ErrorCode.UNAUTHORIZED);
        var result = await reviewService.UpdateReview(id, request);
        return ApiResponse<ReviewResponse>.Success(result);
    }

    [HttpDelete("{id}")]
    public async Task<ApiResponse<string>> DeleteReview(int id)
    {
        var username = User.Identity?.Name ?? throw new AppException(ErrorCode.UNAUTHORIZED);
        await reviewService.DeleteReview(id);
        return ApiResponse<string>.Success();
    }
}
