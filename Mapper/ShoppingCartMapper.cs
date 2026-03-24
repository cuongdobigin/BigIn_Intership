using AutoMapper;
using MyApp.Application.Dto.Response;
using Myapp.Domain.Entity;

namespace webApi.Mapper;

public class ShoppingCartMapper : Profile
{
    public ShoppingCartMapper()
    {
        CreateMap<ShoppingCart, ShoppingCartResponse>()
            .ForMember(dest => dest.BookId, opt => opt.MapFrom(src => src.Books.Id))
            .ForMember(dest => dest.BookName, opt => opt.MapFrom(src => src.Books.Name))
            .ForMember(dest => dest.BookPrice, opt => opt.MapFrom(src => src.Books.Price));
    }
}
