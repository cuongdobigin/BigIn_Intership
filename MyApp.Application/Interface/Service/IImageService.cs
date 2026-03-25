using MyApp.Application.Dto.Request;
using MyApp.Application.Dto.Response;

namespace MyApp.Application.Interface.Service;

public interface IImageService
{
    Task<ImageResponse> CreateImage(ImageRequest request);
    Task<List<ImageResponse>> CreateImages(BulkImageRequest request);
    Task<ImageResponse> UpdateImage(int id, ImageRequest request);
    Task DeleteImage(int id);
    Task<ImageResponse> GetImageById(int id);
    Task<List<ImageResponse>> GetImagesByBookId(int bookId);
}
