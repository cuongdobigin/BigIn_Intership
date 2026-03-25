using AutoMapper;
using MyApp.Application.Dto.Request;
using MyApp.Application.Dto.Response;
using Myapp.Domain.Entity;

namespace webApi.Mapper;

public class BookMapper : Profile
{
    public BookMapper()
    {
        CreateMap<BookRequest, Book>();
        CreateMap<Book, BookResponse>();
    }
}