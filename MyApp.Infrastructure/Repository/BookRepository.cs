

using Microsoft.EntityFrameworkCore;
using MyApp.Application.Interface.Repository;
using Myapp.Domain.Entity;
using webApi.Infrastructure.Persistence;

namespace webApi.Repository;

public class BookRepository(AppDbContext context):IBookRepository
{
    public async Task<Book> createBook(Book book)
    {
        context.Books.Add(book);
        await context.SaveChangesAsync();
        return book;
    }

    public async Task<Book> updateBook(Book book)
    {
        context.Books.Update(book);
        await context.SaveChangesAsync();
        return book;
    }

    public async Task<Book?> findById(int Id)
    {
        return await context.Books.Include(b => b.Images).FirstOrDefaultAsync(a => a.Id == Id);
    }
    public async Task<(List<Book>,int)> getAllBooks(int page,int pageSize)
    {
        var query = context.Books
            .Include(b => b.Images)
            .Where(b => b.IsActive == true);
        int totalItems = await query.CountAsync();
        List<Book> books = await query
                            .Skip((page - 1) * pageSize)
                            .Take(pageSize)
                            .ToListAsync();
        return (books, totalItems);
    }

    public async Task<(List<Book>,int)> getAllBooks_TypeBook(int type,int page,int pageSize)
    {
        var query = context.Books
            .Include(b => b.TypeBook)
            .Include(b => b.Images)
            .Where(b => b.IsActive == true && b.TypeBook.TypeId == type);
        int totalItems = await query.CountAsync();
        return (query.Skip((page - 1) * pageSize).Take(pageSize).ToList(), totalItems);
    }
}