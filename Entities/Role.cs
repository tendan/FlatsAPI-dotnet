using FlatsAPI.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FlatsAPI.Entities
{
    public class Role : INameable
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        public ICollection<Account> Accounts { get; set; } = new List<Account>();
        public ICollection<Permission> Permissions { get; set; } = new List<Permission>();
    }
}
