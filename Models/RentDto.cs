using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlatsAPI.Models
{
    public class RentDto
    {
        public DateTime CreationDate { get; set; }
        public DateTime PayDate { get; set; }
        public bool Paid { get; set; }
        public float Price { get; set; }
        public float PriceWithTax { get; set; }
        /*public int FlatId { get; set; }
        public virtual FlatInRentDto flat { get; set; }*/

        public virtual AccountDto Owner { get; set; }

        public virtual ICollection<AccountDto> Tenants { get; set; }

    }
}
