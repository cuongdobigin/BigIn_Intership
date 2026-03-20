using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using webApi.Domain.Entity;
using webApi.Dto.Request;
using webApi.Dto.Response;
using webApi.Service.Interface;

namespace webApi.Controller;
[ApiController]
[Route("api/type-books")]
[Authorize]
public class TypeBookController(ITypeBookService typeBookService) : ControllerBase
{
    [HttpGet]
    public async Task<ApiResponse<List<TypeBookResponse>>> FindAll()
    {
        return ApiResponse<List<TypeBookResponse>>.Success(await typeBookService.findAllTypeBooks());
    }

    [HttpGet("{id}")]
    public async Task<ApiResponse<TypeBookResponse>> FindById(int id)
    {
        return ApiResponse<TypeBookResponse>.Success(await typeBookService.findTypeBookById(id));
    }

    [HttpPost]
    
    public async Task<ApiResponse<TypeBookResponse>> CreateTypeBook([FromBody] TypeBookRequest request)
    {
        return ApiResponse<TypeBookResponse>.Success(await typeBookService.createTypeBook(request));
    }

    [HttpPut("{id}")]
    public async Task<ApiResponse<TypeBookResponse>> UpdateTypeBook(int id, [FromBody] TypeBookUpdateRequest request)
    {
        return ApiResponse<TypeBookResponse>.Success(await typeBookService.updateTypeBook(request,id));
    }
}