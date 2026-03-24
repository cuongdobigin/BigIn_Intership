using AutoMapper;
using MyApp.Application.Dto.Request;

namespace webApi.Mapper;

public class AccountMapper: Profile
{
    public AccountMapper()
    {
        CreateMap<RegisterRequest, Account>();
    }
}