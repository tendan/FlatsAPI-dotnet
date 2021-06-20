using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlatsAPI.Models
{
    public class BlockOfFlatsDto : BlockOfFlatsInFlatDto
    {
        public virtual ICollection<FlatInBlockOfFlatsDto> Flats { get; set; }
    }
}
