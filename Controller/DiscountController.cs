using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyApp.Application.Dto.Request;
using MyApp.Application.Dto.Response;
using MyApp.Application.Interface.Service;

namespace webApi.Controller;

[ApiController]
[Route("api/discounts")]
public class DiscountController(IDiscountService discountService) : ControllerBase
{
    [Authorize(Roles = "admin")]
    [HttpPost]
    public async Task<ApiResponse<DiscountResponse>> CreateDiscount([FromBody] DiscountRequest request)
    {
        var result = await discountService.CreateDiscount(request);
        return ApiResponse<DiscountResponse>.Success(result);
    }
    
    [HttpGet]
    public async Task<ApiResponse<List<DiscountResponse>>> GetAllDiscounts()
    {
        var result = await discountService.GetAllDiscounts();
        return ApiResponse<List<DiscountResponse>>.Success(result);
    }
    
    [HttpGet("{id}")]
    public async Task<ApiResponse<DiscountResponse>> GetDiscountById(int id)
    {
        var result = await discountService.GetDiscountById(id);
        return ApiResponse<DiscountResponse>.Success(result);
    }

    [Authorize(Roles = "admin")]
    [HttpPut("{id}")]
    public async Task<ApiResponse<DiscountResponse>> UpdateDiscount(int id, [FromBody] DiscountRequest request)
    {
        var result = await discountService.UpdateDiscount(id, request);
        return ApiResponse<DiscountResponse>.Success(result);
    }
    
    [Authorize(Roles = "admin")]
    [HttpDelete("{id}")]
    public async Task<ApiResponse<string>> DeleteDiscount(int id)
    {
        await discountService.DeleteDiscount(id);
        return ApiResponse<string>.Success();
    }
}
