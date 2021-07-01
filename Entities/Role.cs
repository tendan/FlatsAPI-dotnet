using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FlatsAPI.Entities
{
    public class Role
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        public ICollection<Account> Accounts { get; set; }
        public ICollection<Permission> Permissions { get; set; }
    }
}
