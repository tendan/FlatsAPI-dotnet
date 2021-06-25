using FlatsAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlatsAPI.Models
{
    public class FlatInBlockOfFlatsDto
    {
        public int Id { get; set; }
        public int Area { get; set; }
        public int Number { get; set; }
        public int NumberOfRooms { get; set; }
        public int Floor { get; set; }

        public int? OwnerId { get; set; }
        public virtual AccountDto Owner { get; set; }

        public virtual ICollection<AccountDto> Tenants { get; set; }

        public OwnerShip? OwnerShip { get; set; }

        public float PriceWhenBought { get; set; }
        public float? PricePerMeterSquaredWhenRented { get; set; }
    }
}
