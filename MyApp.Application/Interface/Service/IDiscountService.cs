using MyApp.Application.Dto.Request;
using MyApp.Application.Dto.Response;

namespace MyApp.Application.Interface.Service;

public interface IDiscountService
{
    Task<DiscountResponse> CreateDiscount(DiscountRequest request);
    Task<DiscountResponse> UpdateDiscount(int id, DiscountRequest request);
    Task DeleteDiscount(int id);
    Task<DiscountResponse> GetDiscountById(int id);
    Task<List<DiscountResponse>> GetAllDiscounts();
}
