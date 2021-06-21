using AutoMapper;
using FlatsAPI.Entities;
using FlatsAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlatsAPI.Middleware.Mappings
{
    public class AccountMappingProfile : Profile
    {
        public AccountMappingProfile()
        {
            CreateMap<Account, AccountDto>()
                .ForMember(m => m.RoleName, c => c.MapFrom(s => s.Role.Name));
            CreateMap<CreateAccountDto, Account>();
        }
    }
}
