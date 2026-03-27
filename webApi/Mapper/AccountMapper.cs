using AutoMapper;
using MyApp.Application.Dto.Request;
using MyApp.Application.Dto.Response;

namespace webApi.Mapper;

public class AccountMapper: Profile
{
    public AccountMapper()
    {
        CreateMap<RegisterRequest, Account>();
        CreateMap<Account, AccountResponse>();
    }
}