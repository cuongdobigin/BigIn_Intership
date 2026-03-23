using AutoMapper;
using MyApp.Application.Dto.Request;
using MyApp.Application.Dto.Response;
using MyApp.Application.Interface.Repository;
using MyApp.Application.Interface.Service;
using Myapp.Domain.Entity;
using Myapp.Domain.Exceptions;

namespace MyApp.Application.Service;

public class ImageService(
    IImageRepository imageRepository,
    IBookRepository bookRepository,
    IMapper mapper) : IImageService
{
    public async Task<ImageResponse> CreateImage(ImageRequest request)
    {
        var book = await bookRepository.findById(request.BookId) 
                   ?? throw new AppException(ErrorCode.BOOK_NOT_FOUND);

        var image = mapper.Map<Image>(request);
        image.Book = book;

        await imageRepository.CreateImage(image);
        return mapper.Map<ImageResponse>(image);
    }

    public async Task<List<ImageResponse>> CreateImages(BulkImageRequest request)
    {
        var book = await bookRepository.findById(request.BookId) 
                   ?? throw new AppException(ErrorCode.BOOK_NOT_FOUND);

        var images = request.Links.Select(link => new Image 
        { 
            Link = link, 
            Book = book 
        }).ToList();

        await imageRepository.CreateImages(images);
        return mapper.Map<List<ImageResponse>>(images);
    }

    public async Task<ImageResponse> UpdateImage(int id, ImageRequest request)
    {
        var image = await imageRepository.GetImageById(id) 
                    ?? throw new AppException(ErrorCode.IMAGE_NOT_FOUND);

        mapper.Map(request, image);
        
        if (image.Book.Id != request.BookId)
        {
            var book = await bookRepository.findById(request.BookId) 
                       ?? throw new AppException(ErrorCode.BOOK_NOT_FOUND);
            image.Book = book;
        }

        await imageRepository.UpdateImage(image);
        return mapper.Map<ImageResponse>(image);
    }

    public async Task DeleteImage(int id)
    {
        var image = await imageRepository.GetImageById(id) 
                    ?? throw new AppException(ErrorCode.IMAGE_NOT_FOUND);

        await imageRepository.DeleteImage(image);
    }

    public async Task<ImageResponse> GetImageById(int id)
    {
        var image = await imageRepository.GetImageById(id) 
                    ?? throw new AppException(ErrorCode.IMAGE_NOT_FOUND);

        return mapper.Map<ImageResponse>(image);
    }

    public async Task<List<ImageResponse>> GetImagesByBookId(int bookId)
    {
        var images = await imageRepository.GetImagesByBookId(bookId);
        return mapper.Map<List<ImageResponse>>(images);
    }
}
