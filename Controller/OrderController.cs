using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyApp.Application.Dto.Request;
using MyApp.Application.Dto.Response;
using MyApp.Application.Interface.Service;

namespace webApi.Controller;

[Authorize]
[ApiController]
[Route("api/orders")]
public class OrderController(IOrderService orderService) : ControllerBase
{
    [HttpPost]
    public async Task<ApiResponse<OrderResponse>> CreateOrder([FromBody] OrderRequest request)
    {
        var result = await orderService.CreateOrder(request);
        return ApiResponse<OrderResponse>.Success(result);
    }
    
    [HttpGet]
    public async Task<ApiResponse<List<OrderResponse>>> GetMyOrders()
    {
        var result = await orderService.GetMyOrders();
        return ApiResponse<List<OrderResponse>>.Success(result);
    }

    [HttpGet("{id}")]
    public async Task<ApiResponse<OrderResponse>> GetOrderById(int id)
    {
        var result = await orderService.GetOrderById(id);
        return ApiResponse<OrderResponse>.Success(result);
    }

    [HttpDelete("{id}")]
    public async Task<ApiResponse<string>> CancelOrder(int id)
    {
        await orderService.CancelOrder(id);
        return ApiResponse<string>.Success();
    }
}
