using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using FlatsAPI.Models;

namespace FlatsAPI.Entities
{
    public class Flat
    {
        public int Id { get; set; }

        [Required]
        public int Area { get; set; }
        [Required]
        public int Number { get; set; }
        [Required]
        public int NumberOfRooms { get; set; }
        [Required]
        public int Floor { get; set; }

        [Required]
        public int BlockOfFlatsId { get; set; }
        public virtual BlockOfFlats BlockOfFlats { get; set; }
        
        public int? OwnerId { get; set; }
        public virtual Account Owner { get; set; }
        
        public virtual ICollection<Account> Tenants { get; set; }

        [Required]
        public float PriceWhenBought { get; set; }
        public float? PricePerMeterSquaredWhenRented { get; set; }

        public virtual ICollection<Rent> Rents { get; set; }
    }
}
