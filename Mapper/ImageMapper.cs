using AutoMapper;
using MyApp.Application.Dto.Request;
using MyApp.Application.Dto.Response;
using Myapp.Domain.Entity;

namespace webApi.Mapper;

public class ImageMapper : Profile
{
    public ImageMapper()
    {
        CreateMap<ImageRequest, Image>()
            .ForMember(dest => dest.Book, opt => opt.Ignore()); // Sẽ gán thủ công trong service
        
        CreateMap<Image, ImageResponse>();
    }
}
