
using webApi.Domain.Entity;
using webApi.Dto.Request;
using webApi.Dto.Response;

namespace webApi.Service.Interface;

public interface ITypeBookService
{
    Task<TypeBookResponse> createTypeBook(TypeBookRequest typeBookRequest);
    Task<TypeBookResponse?> findTypeBookById(int id);
    Task<List<TypeBookResponse>> findAllTypeBooks();
    
    Task<TypeBookResponse> updateTypeBook(TypeBookUpdateRequest typeBookUpdateRequest,int id);
    
}