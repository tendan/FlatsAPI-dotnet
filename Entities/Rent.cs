using FlatsAPI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;



namespace FlatsAPI.Entities
{   
    public enum PropertyTypes
    {
        Flat,
        BlockOfFlats
    }
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
        public OwnerShip OwnerShip { get; set; }

        [Required]
        public float PriceWithTax { get; set; }

        [Required]
        public int RentIssuerId { get; set; }
        public virtual Account RentIssuer { get; set; }

        public int PropertyId { get; set; }
        public virtual PropertyTypes Property { get; set; }
    }
}
