using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyApp.Application.Dto.Request;
using MyApp.Application.Dto.Response;
using MyApp.Application.Interface.Service;

namespace webApi.Controller;

[Authorize]
[ApiController]
[Route("api/images")]
public class ImageController(IImageService imageService) : ControllerBase
{
    [HttpPost]
    public async Task<ApiResponse<ImageResponse>> CreateImage([FromBody] ImageRequest request)
    {
        var result = await imageService.CreateImage(request);
        return ApiResponse<ImageResponse>.Success(result);
    }

    [HttpPost("bulk")]
    public async Task<ApiResponse<List<ImageResponse>>> CreateImages([FromBody] BulkImageRequest request)
    {
        var result = await imageService.CreateImages(request);
        return ApiResponse<List<ImageResponse>>.Success(result);
    }

    [HttpGet("{id}")]
    public async Task<ApiResponse<ImageResponse>> GetImageById(int id)
    {
        var result = await imageService.GetImageById(id);
        return ApiResponse<ImageResponse>.Success(result);
    }

    [HttpGet("book/{bookId}")]
    public async Task<ApiResponse<List<ImageResponse>>> GetImagesByBookId(int bookId)
    {
        var result = await imageService.GetImagesByBookId(bookId);
        return ApiResponse<List<ImageResponse>>.Success(result);
    }

    [HttpPut("{id}")]
    public async Task<ApiResponse<ImageResponse>> UpdateImage(int id, [FromBody] ImageRequest request)
    {
        var result = await imageService.UpdateImage(id, request);
        return ApiResponse<ImageResponse>.Success(result);
    }

    [HttpDelete("{id}")]
    public async Task<ApiResponse<string>> DeleteImage(int id)
    {
        await imageService.DeleteImage(id);
        return ApiResponse<string>.Success();
    }
}
