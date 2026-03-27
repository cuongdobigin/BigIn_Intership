

using Microsoft.EntityFrameworkCore;
using MyApp.Application.Interface.Repository;
using Myapp.Domain.Entity;
using webApi.Infrastructure.Persistence;

namespace webApi.Repository;

public class TypeBookRepository(AppDbContext appDbContext):ITypeBookRepository
{
    public async Task<TypeBook> createTypeBook(TypeBook typeBook)
    {
        appDbContext.TypeBooks.Add(typeBook);
        await appDbContext.SaveChangesAsync();
        return typeBook;
    }

    public async Task<List<TypeBook>> findAllTypeBooks()
    {
        return await appDbContext.TypeBooks
            .Where(a=> a.IsActive==true)
            .ToListAsync();
    }
    public async Task<List<TypeBook>> findAllTypeBooks_Admin()
    {
        return await appDbContext.TypeBooks
            .ToListAsync();
    }

    public async Task<TypeBook?> findTypeBookById(int id)
    {
        return await appDbContext.TypeBooks
            .FirstOrDefaultAsync(u=> u.TypeId == id);
    }

    public async Task<bool> existTypeBookByName(string name)
    {
        return await appDbContext.TypeBooks.AnyAsync(a => a.Name == name);
    }

    public async Task<TypeBook> updateTypeBook(TypeBook typeBook)
    {
        appDbContext.TypeBooks.Update(typeBook);
        await appDbContext.SaveChangesAsync();
        return typeBook;
    }

    public async Task<TypeBook?> findTypeBookByName(string name)
    {
        return await appDbContext.TypeBooks.FirstOrDefaultAsync(a => a.Name == name);
    }
}