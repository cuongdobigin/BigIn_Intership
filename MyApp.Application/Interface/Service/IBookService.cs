using MyApp.Application.Dto.Request;
using MyApp.Application.Dto.Response;

namespace MyApp.Application.Interface.Service;
public interface IBookService 
{
    Task<PageResponse<BookResponse>> GetAllBook(PageRequest pageRequest);
    Task<BookResponse?> GetBookById(int id);
    Task<BookResponse> UpdateBook(BookRequest request,int bookId);
    Task<BookResponse> CreateBook(BookRequest request);
    Task<PageResponse<BookResponse>> GetAllBook_TypeBook(int typeId,PageRequest pageRequest);
}