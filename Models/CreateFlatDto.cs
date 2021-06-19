using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlatsAPI.Models
{
    public class CreateFlatDto
    {
        public int Area { get; set; }
        public int Number { get; set; }
        public int NumberOfRooms { get; set; }
        public int Floor { get; set; }
        public int BlockOfFlatsId { get; set; }
        public int OwnerId { get; set; }

        public float PriceWhenBought { get; set; }
        public float PricePerMeterSquaredWhenRented { get; set; }
    }
}
