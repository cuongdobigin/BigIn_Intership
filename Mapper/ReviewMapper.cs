using AutoMapper;
using MyApp.Application.Dto.Request;
using MyApp.Application.Dto.Response;
using Myapp.Domain.Entity;

namespace webApi.Mapper;

public class ReviewMapper : Profile
{
    public ReviewMapper()
    {
        CreateMap<Review, ReviewResponse>()
            .ForMember(dest => dest.AccountUsername, opt => opt.MapFrom(src => src.Account.username))
            .ForMember(dest => dest.BookId, opt => opt.MapFrom(src => src.Book.Id));
        CreateMap<ReviewRequest, Review>();
    }
}
