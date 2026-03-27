using AutoMapper;
using MyApp.Application.Dto.Request;
using MyApp.Application.Dto.Response;
using Myapp.Domain.Entity;

namespace webApi.Mapper;

public class BookMapper : Profile
{
    public BookMapper()
    {
        CreateMap<BookRequest, Book>()
            .ForMember(dest => dest.type_book_Id, opt => opt.MapFrom(src => src.TypeBookId));
        CreateMap<Book, BookResponse>()
            .ForMember(dest => dest.TypeBookId, opt => opt.MapFrom(src => src.type_book_Id));
    }
}