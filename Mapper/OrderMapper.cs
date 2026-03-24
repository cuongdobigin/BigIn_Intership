using AutoMapper;
using MyApp.Application.Dto.Response;
using Myapp.Domain.Entity;

namespace webApi.Mapper;

public class OrderMapper : Profile
{
    public OrderMapper()
    {
        CreateMap<Order, OrderResponse>()
            .ForMember(dest => dest.DiscountPercent, opt => opt.MapFrom(src => src.Discount != null ? src.Discount.Percent : 0));
    }
}
