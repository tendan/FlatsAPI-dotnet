using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FlatsAPI.Entities;
using FlatsAPI.Models;

namespace FlatsAPI.Middleware.Mappings
{
    public class FlatMappingProfile : Profile
    {
        public FlatMappingProfile()
        {
            CreateMap<Flat, FlatInRentDto>();
            CreateMap<Flat, FlatDto>();
            CreateMap<CreateFlatDto, Flat>();
        }
    }
}
