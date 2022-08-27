using FlatsAPI.Models.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FlatsAPI.Entities;

public class Permission : INameable
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    public ICollection<Role> Roles { get; set; } = new List<Role>();
}
