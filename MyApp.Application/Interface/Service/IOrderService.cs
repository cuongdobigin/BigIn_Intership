using MyApp.Application.Dto.Request;
using MyApp.Application.Dto.Response;

namespace MyApp.Application.Interface.Service;

public interface IOrderService
{
    Task<OrderResponse> CreateOrder(OrderRequest request);
    Task CancelOrder(int id);
    Task<OrderResponse> GetOrderById(int id);
    Task<List<OrderResponse>> GetMyOrders();
}
