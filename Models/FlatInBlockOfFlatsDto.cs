using FlatsAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlatsAPI.Models
{
    public class FlatInBlockOfFlatsDto : FlatInRentDto
    {
        public virtual AccountDto Owner { get; set; }

        public virtual ICollection<AccountDto> Tenants { get; set; }
    }
}
