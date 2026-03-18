using AutoMapper;
using webApi.Domain.Entity;
using webApi.Dto.Request;

namespace webApi.Mapper;

public class AccountMapper: Profile
{
    public AccountMapper()
    {
        CreateMap<RegisterRequest, Account>();
    }
}