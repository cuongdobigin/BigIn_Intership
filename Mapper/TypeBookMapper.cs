using AutoMapper;
using webApi.Domain.Entity;
using webApi.Dto.Request;
using webApi.Dto.Response;

namespace webApi.Mapper;

public class TypeBookMapper:Profile
{
    public TypeBookMapper()
    {
        CreateMap<TypeBookRequest, TypeBook>();
        CreateMap<TypeBook, TypeBookResponse>();
        CreateMap<TypeBookUpdateRequest, TypeBook>();
    }
}