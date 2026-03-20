using AutoMapper;
using webApi.Domain.Entity;
using webApi.Dto.Request;
using webApi.Dto.Response;

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