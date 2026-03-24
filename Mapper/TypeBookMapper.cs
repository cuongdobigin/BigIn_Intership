

using AutoMapper;
using MyApp.Application.Dto.Request;
using MyApp.Application.Dto.Response;
using Myapp.Domain.Entity;

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