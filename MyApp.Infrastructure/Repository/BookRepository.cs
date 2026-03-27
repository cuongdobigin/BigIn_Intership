

using Microsoft.EntityFrameworkCore;
using MyApp.Application.Dto.Request;
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
    public async Task<(List<Book>,int)> getAllBooks(int page,int pageSize, string? search = null)
    {
        var query = context.Books
            .Include(b => b.Images)
            .Where(b => b.IsActive == true);

        if (!string.IsNullOrEmpty(search))
        {
            query = query.Where(b => b.Name.Contains(search) || b.Description.Contains(search));
        }

        int totalItems = await query.CountAsync();
        List<Book> books = await query
                            .Skip((page - 1) * pageSize)
                            .Take(pageSize)
                            .ToListAsync();
        return (books, totalItems);
    }

    public async Task<(List<Book>,int)> getAllBooks_TypeBook(int type,int page,int pageSize, string? search = null)
    {
        var query = context.Books
            .Include(b => b.TypeBook)
            .Include(b => b.Images)
            .Where(b => b.IsActive == true && b.TypeBook.TypeId == type);

        if (!string.IsNullOrEmpty(search))
        {
            query = query.Where(b => b.Name.Contains(search) || b.Description.Contains(search));
        }

        int totalItems = await query.CountAsync();
        return (await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(), totalItems);
    }
    
    public async Task<(List<Book>,int)> getAllBooks_TypeBook_admin(int type,int page,int pageSize, string? search = null)
    {
        var query = context.Books
            .Include(b => b.TypeBook)
            .Include(b => b.Images)
            .Where( b=>b.TypeBook.TypeId == type);

        if (!string.IsNullOrEmpty(search))
        {
            query = query.Where(b => b.Name.Contains(search) || b.Description.Contains(search));
        }

        int totalItems = await query.CountAsync();
        return (await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(), totalItems);
    }
    
    
    public async Task<List<Book>> SearchAsync(SearchFilterDto filter)
    {
        var query = context.Books
            .Include(b => b.TypeBook).AsQueryable();
        if (filter.Keywords.Any())
        {
            query = query.Where(p =>
                filter.Keywords.Any(kw =>
                    p.Name.ToLower().Contains(kw.ToLower()) ||
                    p.Description.ToLower().Contains(kw.ToLower())));
        }
        
        if (!string.IsNullOrEmpty(filter.TypeBook))
        {
            query = query.Where(p =>
                p.TypeBook.Name.ToLower().Contains(filter.TypeBook.ToLower()));
        }
        
        if (filter.MinPrice.HasValue)
            query = query.Where(p => p.Price >= filter.MinPrice.Value);

        if (filter.MaxPrice.HasValue)
            query = query.Where(p => p.Price <= filter.MaxPrice.Value);
        
        return await query.Take(10).ToListAsync();
    }
}