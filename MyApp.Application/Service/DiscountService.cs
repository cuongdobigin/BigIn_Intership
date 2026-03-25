using AutoMapper;
using MyApp.Application.Dto.Request;
using MyApp.Application.Dto.Response;
using MyApp.Application.Interface.Repository;
using MyApp.Application.Interface.Service;
using Myapp.Domain.Entity;
using Myapp.Domain.Exceptions;

namespace MyApp.Application.Service;

public class DiscountService(
    IDiscountRepository discountRepository,
    IMapper mapper) : IDiscountService
{
    public async Task<DiscountResponse> CreateDiscount(DiscountRequest request)
    {
        request.StartDate = DateTime.SpecifyKind(request.StartDate, DateTimeKind.Utc);
        request.EndDate = DateTime.SpecifyKind(request.EndDate, DateTimeKind.Utc);
        if (await discountRepository.ExistsByPercent(request.Percent, request.StartDate, request.EndDate))
            throw new AppException(ErrorCode.DISCOUNT_EXISTED);

        Discount discount= mapper.Map<Discount>(request);

        await discountRepository.Create(discount);
        return mapper.Map<DiscountResponse>(discount);
    }

    public async Task<DiscountResponse> UpdateDiscount(int id, DiscountRequest request)
    {
        request.StartDate = DateTime.SpecifyKind(request.StartDate, DateTimeKind.Utc);
        request.EndDate = DateTime.SpecifyKind(request.EndDate, DateTimeKind.Utc);
        var discount = await discountRepository.GetById(id)
                       ?? throw new AppException(ErrorCode.DISCOUNT_NOT_FOUND);

        mapper.Map(request, discount);

        await discountRepository.Update(discount);
        return mapper.Map<DiscountResponse>(discount);
    }

    public async Task DeleteDiscount(int id)
    {
        var discount = await discountRepository.GetById(id)
                       ?? throw new AppException(ErrorCode.DISCOUNT_NOT_FOUND);

        await discountRepository.Delete(discount);
    }

    public async Task<DiscountResponse> GetDiscountById(int id)
    {
        var discount = await discountRepository.GetById(id)
                       ?? throw new AppException(ErrorCode.DISCOUNT_NOT_FOUND);

        return mapper.Map<DiscountResponse>(discount);
    }

    public async Task<List<DiscountResponse>> GetAllDiscounts()
    {
        var discounts = await discountRepository.GetAll();
        return mapper.Map<List<DiscountResponse>>(discounts);
    }
}
