using AutoMapper;
using MyApp.Application.Dto.Request;
using MyApp.Application.Dto.Response;
using MyApp.Application.Interface.Repository;
using MyApp.Application.Interface.Service;
using Myapp.Domain.Entity;
using Myapp.Domain.Exceptions;


namespace MyApp.Application.Service;

public class TypeBookService(ITypeBookRepository typeBookRepository,IMapper mapper):ITypeBookService
{
    public async Task<TypeBookResponse> createTypeBook(TypeBookRequest typeBookRequest)
    {
        if (await typeBookRepository.existTypeBookByName(typeBookRequest.Name))
            throw new AppException(ErrorCode.TYPE_BOOK_EXISTED);
        var typeBook = mapper.Map<TypeBook>(typeBookRequest);
        await typeBookRepository.createTypeBook(typeBook);
        return mapper.Map<TypeBookResponse>(typeBook);
    }


    public async Task<List<TypeBookResponse>> findAllTypeBooks()
    {
         return mapper.Map<List<TypeBookResponse>>(await typeBookRepository.findAllTypeBooks());
    }

    public async Task<TypeBookResponse?> findTypeBookById(int id)
    {
        TypeBook typeBook = await typeBookRepository.findTypeBookById(id)
            ?? throw new AppException(ErrorCode.TYPE_BOOK_NOT_FOUND);
        if (!typeBook.IsActive)
            throw new AppException(ErrorCode.TYPE_BOOK_NOT_FOUND);
        return mapper.Map<TypeBookResponse>(typeBook);
    }

    public async Task<TypeBookResponse> updateTypeBook(TypeBookUpdateRequest typeBookUpdateRequest,int id)
    {
        TypeBook? typeBook = await typeBookRepository.findTypeBookById(id);
        if (typeBook == null)
            throw new AppException(ErrorCode.TYPE_BOOK_NOT_FOUND);
            
        if (await typeBookRepository.existTypeBookByName(typeBookUpdateRequest.Name)&&typeBook.Name!=typeBookUpdateRequest.Name)
            throw new AppException(ErrorCode.TYPE_BOOK_EXISTED);
            
        mapper.Map(typeBookUpdateRequest, typeBook);
        return mapper.Map<TypeBookResponse>(await typeBookRepository.updateTypeBook(typeBook));
    }
}