using webApi.Domain.Entity;
using webApi.Dto.Response;

namespace webApi.Infrastructure.Persistence.Repository;

public interface ITypeBookRepository
{
    Task<TypeBook> createTypeBook(TypeBook typeBook);
    Task<TypeBook?> findTypeBookById(int id);
    Task<List<TypeBook>> findAllTypeBooks();
    Task<bool> existTypeBookByName(string name);
    Task<TypeBook> updateTypeBook( TypeBook typeBook);
    Task<TypeBook?> findTypeBookByName(string name);
    
}