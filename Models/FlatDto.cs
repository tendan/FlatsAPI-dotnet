using FlatsAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlatsAPI.Models
{
    public class FlatDto : FlatInBlockOfFlatsDto
    {
        public int BlockOfFlatsId { get; set; }
        public virtual BlockOfFlatsInFlatDto BlockOfFlats { get; set; }
    }
}
