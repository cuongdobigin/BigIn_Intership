using Microsoft.EntityFrameworkCore;
using MyApp.Application.Interface.Repository;
using Myapp.Domain.Entity;
using webApi.Infrastructure.Persistence;

namespace webApi.Repository;

public class ImageRepository(AppDbContext context) : IImageRepository
{
    public async Task<Image> CreateImage(Image image)
    {
        await context.Images.AddAsync(image);
        await context.SaveChangesAsync();
        return image;
    }

    public async Task<List<Image>> CreateImages(List<Image> images)
    {
        await context.Images.AddRangeAsync(images);
        await context.SaveChangesAsync();
        return images;
    }

    public async Task<Image> UpdateImage(Image image)
    {
        context.Images.Update(image);
        await context.SaveChangesAsync();
        return image;
    }

    public async Task DeleteImage(Image image)
    {
        context.Images.Remove(image);
        await context.SaveChangesAsync();
    }

    public async Task<Image?> GetImageById(int id)
    {
        return await context.Images.Include(i => i.Book).FirstOrDefaultAsync(i => i.Id == id);
    }

    public async Task<List<Image>> GetImagesByBookId(int bookId)
    {
        return await context.Images.Where(i => i.Book.Id == bookId).ToListAsync();
    }
}
