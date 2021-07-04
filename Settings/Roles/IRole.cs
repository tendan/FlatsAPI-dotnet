using FlatsAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlatsAPI.Settings.Roles
{
    interface IRole
    {
        public static string Name { get; }
        public static ICollection<string> Permissions { get; }
    }
}
