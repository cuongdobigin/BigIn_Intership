using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyApp.Application.Dto.Request;
using MyApp.Application.Dto.Response;
using MyApp.Application.Interface.Service;


namespace webApi.Controller;

[Authorize]
[ApiController]
[Route("api/books")]
public class BookController(IBookService bookService) : ControllerBase
{
    

    [HttpPost]
    public async Task<ApiResponse<BookResponse>> CreateBook([FromBody] BookRequest request)
    {
        var result = await bookService.CreateBook(request);
        return ApiResponse<BookResponse>.Success(result);
    }

    [HttpGet]
    public async Task<ApiResponse<PageResponse<BookResponse>>> GetAllBook([FromQuery] PageRequest request)
    {
        var result = await bookService.GetAllBook(request);
        return ApiResponse<PageResponse<BookResponse>>.Success(result);
    }
    [HttpGet("type-books/{id}")]
    public async Task<ApiResponse<PageResponse<BookResponse>>> GetAllBook_TypeBook([FromQuery] PageRequest request,int id)
    {
        var result = await bookService.GetAllBook_TypeBook(id,request);
        return ApiResponse<PageResponse<BookResponse>>.Success(result);
    }
    [HttpGet("{id}")]
    public async Task<ApiResponse<BookResponse>> GetBookById(int id)
    {
        var result = await bookService.GetBookById(id);
        return ApiResponse<BookResponse>.Success(result);
    }

    [HttpPut("{id}")]
    public async Task<ApiResponse<BookResponse>> UpdateBook([FromBody] BookRequest request, int id)
    {
        var result = await bookService.UpdateBook(request, id);
        return ApiResponse<BookResponse>.Success(result);
    }
}