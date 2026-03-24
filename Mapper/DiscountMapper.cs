using AutoMapper;
using MyApp.Application.Dto.Request;
using MyApp.Application.Dto.Response;
using Myapp.Domain.Entity;

namespace webApi.Mapper;

public class DiscountMapper : Profile
{
    public DiscountMapper()
    {
        CreateMap<Discount, DiscountResponse>();
        CreateMap<DiscountRequest, Discount>();
    }
}
