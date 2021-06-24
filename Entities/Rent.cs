using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FlatsAPI.Entities
{
    public class Rent
    {
        public int Id { get; set; }

        [Required]
        public DateTime CreationDate { get; set; }
        [Required]
        public DateTime PayDate { get; set; }
        [Required]
        public bool Paid { get; set; }

        [Required]
        public float Price { get; set; }

        [Required]
        public float PriceWithTax { get; set; }

        public int FlatId { get; set; }
        public virtual Flat Flat { get; set; }

        public int OwnerId { get; set; }
        public virtual Account Owner { get; set; }

        public virtual ICollection<Account> Tenants { get; set; }
    }
}
