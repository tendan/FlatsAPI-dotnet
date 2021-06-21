using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlatsAPI.Models
{
    public class CreateBlockOfFlatsDto 
    {
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public int Floors { get; set; }
        public float Margin { get; set; }
        public int? OwnerId { get; set; }
    }
}
