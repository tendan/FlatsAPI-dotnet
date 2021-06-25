using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlatsAPI.Models
{
    public class FlatDto : FlatInBlockOfFlatsDto
    {
        public virtual BlockOfFlatsDto BlockOfFlats { get; set; }
    }
}
