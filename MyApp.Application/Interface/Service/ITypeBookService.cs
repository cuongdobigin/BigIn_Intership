using MyApp.Application.Dto.Request;
using MyApp.Application.Dto.Response;

namespace MyApp.Application.Interface.Service;

public interface ITypeBookService
{
    Task<TypeBookResponse> createTypeBook(TypeBookRequest typeBookRequest);
    Task<TypeBookResponse?> findTypeBookById(int id);
    Task<List<TypeBookResponse>> findAllTypeBooks();
    Task<List<TypeBookResponse>> findAllTypeBooks_Admin();
    
    Task<TypeBookResponse> updateTypeBook(TypeBookUpdateRequest typeBookUpdateRequest,int id);
    
}