using Myapp.Domain.Entity;

namespace MyApp.Application.Interface.Repository;

public interface IDiscountRepository
{
    Task<Discount> Create(Discount discount);
    Task<Discount> Update(Discount discount);
    Task Delete(Discount discount);
    Task<Discount?> GetById(int id);
    Task<List<Discount>> GetAll();
    Task<bool> ExistsByPercent(decimal percent,DateTime startDate, DateTime endDate);
}
