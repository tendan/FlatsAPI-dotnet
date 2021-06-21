using AutoMapper;
using FlatsAPI.Entities;
using FlatsAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlatsAPI.Middleware.Mappings
{
    public class BlockOfFlatsMappingProfile : Profile
    {
        public BlockOfFlatsMappingProfile()
        {
            CreateMap<BlockOfFlats, BlockOfFlatsDto>();
            CreateMap<CreateBlockOfFlatsDto, BlockOfFlats>();
        }
    }
}
