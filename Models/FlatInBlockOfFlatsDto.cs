using System.Collections.Generic;

namespace FlatsAPI.Models;

public class FlatInBlockOfFlatsDto : FlatInRentDto
{
    public virtual AccountDto Owner { get; set; }

    public virtual ICollection<AccountDto> Tenants { get; set; }
}
