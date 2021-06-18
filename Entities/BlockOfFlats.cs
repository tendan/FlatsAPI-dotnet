using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace FlatsAPI.Entities
{
    public class BlockOfFlats
    {
        public int Id { get; set; }

        [Required]
        public string Address { get; set; }
        [Required]
        public string PostalCode { get; set; }
        [Required]
        public int Floors { get; set; }
        [Required]
        public float Margin { get; set; }

        public int OwnerId { get; set; }
        public Account Owner { get; set; }

        public virtual ICollection<Flat> Flats { get; set; }
    }
}
