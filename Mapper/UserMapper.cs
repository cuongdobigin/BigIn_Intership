using AutoMapper;
using MyApp.Application.Dto.Request;
using MyApp.Application.Dto.Response;
using Myapp.Domain.Entity;


namespace webApi.Mapper;

public class UserMapper: Profile
{
    public UserMapper()
    {
        CreateMap<UserRequest, User>();
        CreateMap<User, UserResponse>()
            .ForMember(dest => dest.usernameAccount,
                opt => opt.MapFrom(src => src.Account != null ? src.Account.username : null));
    }
}