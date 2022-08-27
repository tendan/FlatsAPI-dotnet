using AutoMapper;
using FlatsAPI.Entities;
using FlatsAPI.Models;

namespace FlatsAPI.Middleware.Mappings;

public class FlatMappingProfile : Profile
{
    public FlatMappingProfile()
    {
        CreateMap<Flat, FlatInRentDto>();
        CreateMap<Flat, FlatInBlockOfFlatsDto>();
        CreateMap<Flat, FlatDto>();
        CreateMap<CreateFlatDto, Flat>();
    }
}
