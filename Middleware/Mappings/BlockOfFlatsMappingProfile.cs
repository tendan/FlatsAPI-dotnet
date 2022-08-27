using AutoMapper;
using FlatsAPI.Entities;
using FlatsAPI.Models;

namespace FlatsAPI.Middleware.Mappings;

public class BlockOfFlatsMappingProfile : Profile
{
    public BlockOfFlatsMappingProfile()
    {
        CreateMap<BlockOfFlats, BlockOfFlatsDto>();
        CreateMap<CreateBlockOfFlatsDto, BlockOfFlats>();
    }
}
