using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FlatsAPI.Entities
{
    public class Account
    {
        public int Id { get; set; }

        [Required]
        public string Email { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public int RoleId { get; set; }
        public virtual Role Role { get; set; }

        public virtual ICollection<Rent> Rents { get; set; } = new List<Rent>();
        public virtual ICollection<Flat> OwnedFlats { get; set; } = new List<Flat>();
        public virtual ICollection<BlockOfFlats> OwnedBlocksOfFlats { get; set; } = new List<BlockOfFlats>();
    }
}
