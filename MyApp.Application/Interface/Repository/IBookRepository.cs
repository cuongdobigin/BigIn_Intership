using MyApp.Application.Dto.Request;
using Myapp.Domain.Entity;

namespace MyApp.Application.Interface.Repository;

public interface IBookRepository
{
    Task<Book> createBook(Book book);
    Task<Book> updateBook(Book book);
    Task<Book?> findById(int Id);
    Task<(List<Book>,int totalItems)> getAllBooks(int page,int pageSize, string? search = null);
    Task<(List<Book>,int totalItems)> getAllBooks_TypeBook(int type,int page,int pageSize, string? search = null);
    Task<(List<Book>,int totalItems)> getAllBooks_TypeBook_admin(int type,int page,int pageSize, string? search = null);
    Task<List<Book>> SearchAsync(SearchFilterDto filterDto);
}