using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlatsAPI.Models
{
    public class BlockOfFlatsInFlatDto
    {
        public int Id { get; set; }

        public string Address { get; set; }
        public string PostalCode { get; set; }
        public int Floors { get; set; }
        public float Margin { get; set; }

        public int? OwnerId { get; set; }
        public virtual AccountDto Owner { get; set; }
    }
}
