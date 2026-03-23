using Myapp.Domain.Entity;

namespace MyApp.Application.Interface.Repository;

public interface IImageRepository
{
    Task<Image> CreateImage(Image image);
    Task<List<Image>> CreateImages(List<Image> images);
    Task<Image> UpdateImage(Image image);
    Task DeleteImage(Image image);
    Task<Image?> GetImageById(int id);
    Task<List<Image>> GetImagesByBookId(int bookId);
}
