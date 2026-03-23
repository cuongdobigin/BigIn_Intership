using AutoMapper;
using MyApp.Application.Dto.Request;
using MyApp.Application.Dto.Response;
using MyApp.Application.Interface.Repository;
using MyApp.Application.Interface.Service;
using Myapp.Domain.Entity;
using Myapp.Domain.Exceptions;


namespace MyApp.Application.Service;

public class BookService(IMapper mapper,IBookRepository bookRepository,ITypeBookRepository typeBookRepository): IBookService
{
    public async Task<BookResponse> CreateBook(BookRequest request)
    {
        TypeBook typeBook = await typeBookRepository.findTypeBookById(request.TypeBookId)??
            throw new AppException(ErrorCode.TYPE_BOOK_NOT_FOUND);
        Book book = mapper.Map<Book>(request);
        book.TypeBook=typeBook;
        await bookRepository.createBook(book);
        return mapper.Map<BookResponse>(book);
    }

    public async Task<PageResponse<BookResponse>> GetAllBook(PageRequest pageRequest)
    {
        var (books, totalItems) =
            await bookRepository.getAllBooks( pageRequest.Page, pageRequest.PageSize);
        return new PageResponse<BookResponse>
        {
            Data = mapper.Map<List<BookResponse>>(books),
            Page = pageRequest.Page,
            PageSize = pageRequest.PageSize,
            TotalItems = totalItems,
            TotalPages = (int)Math.Ceiling((double)totalItems / pageRequest.PageSize)
        };
    }

    public async Task<BookResponse?> GetBookById(int id)  
    {
        Book book=await bookRepository.findById(id) ??
            throw new AppException(ErrorCode.BOOK_NOT_FOUND);
        return mapper.Map<BookResponse>(book);
    }

    public async Task<BookResponse> UpdateBook(BookRequest request,int bookId)
    {
        Book book=await bookRepository.findById(bookId)??throw new AppException(ErrorCode.BOOK_NOT_FOUND);
        mapper.Map(request, book);
        await bookRepository.updateBook(book);
        return mapper.Map<BookResponse>(book);
    }

    public async Task<PageResponse<BookResponse>> GetAllBook_TypeBook(int typeId,PageRequest pageRequest)
    {
        var (books, totalItems) =
            await bookRepository.getAllBooks_TypeBook(typeId, pageRequest.Page, pageRequest.PageSize);
        return new PageResponse<BookResponse>
        {
            Data = mapper.Map<List<BookResponse>>(books),
            Page = pageRequest.Page,
            PageSize = pageRequest.PageSize,
            TotalItems = totalItems,
            TotalPages = (int)Math.Ceiling((double)totalItems / pageRequest.PageSize)
        };
    }
}