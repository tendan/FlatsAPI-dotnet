using AutoMapper;
using FlatsAPI.Entities;
using FlatsAPI.Models;

namespace FlatsAPI.Middleware.Mappings;

public class AccountMappingProfile : Profile
{
    public AccountMappingProfile()
    {
        CreateMap<Account, AccountDto>()
            .ForMember(m => m.RoleName, c => c.MapFrom(s => s.Role.Name))
            .ForMember(m => m.FullName, c => c.MapFrom(s => $"{s.FirstName} {s.LastName}"));
        CreateMap<CreateAccountDto, Account>();
    }
}
