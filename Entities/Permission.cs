using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FlatsAPI.Entities
{
    public class Permission
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public ICollection<Role> Roles { get; set; } = new List<Role>();
    }
}
