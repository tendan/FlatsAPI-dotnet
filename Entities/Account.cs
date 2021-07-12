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
        [Column("Email")]
        public string Email { get; set; }
        [Required]
        [Column("Username")]
        public string Username { get; set; }
        [Required]
        [Column("FirstName")]
        public string FirstName { get; set; }
        [Required]
        [Column("LastName")]
        public string LastName { get; set; }
        [Required]
        [Column("Password")]
        public string Password { get; set; }
        [Required]
        [Column("DateOfBirth")]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [Column("RoleId")]
        public int RoleId { get; set; }
        public virtual Role Role { get; set; }

        public virtual ICollection<Rent> Rents { get; set; } = new List<Rent>();
        [InverseProperty("Owner")]
        public virtual ICollection<Flat> OwnedFlats { get; set; } = new List<Flat>();
        [InverseProperty("Owner")]
        public virtual ICollection<BlockOfFlats> OwnedBlocksOfFlats { get; set; } = new List<BlockOfFlats>();
        public virtual ICollection<Flat> RentedFlats { get; set; } = new List<Flat>();
    }
}
