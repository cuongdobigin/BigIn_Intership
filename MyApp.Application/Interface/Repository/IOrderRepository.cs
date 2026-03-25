using Myapp.Domain.Entity;

namespace MyApp.Application.Interface.Repository;

public interface IOrderRepository
{
    Task<Order> Create(Order order);
    Task<Order> Update(Order order);
    Task<Order?> GetById(int id);
    Task<List<Order>> GetMyOrders(string username);
}
