using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlatsAPI.Models
{
    public class AccountDto
    {
        public string Email { get; set; }
        public string Username { get; set; }
        public RoleDto Role { get; set; }
    }
}
