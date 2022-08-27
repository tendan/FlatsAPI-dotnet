using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlatsAPI.Entities;

public class BlockOfFlats
{
    public int Id { get; set; }

    [Required]
    public string Address { get; set; }
    [Required]
    public string PostalCode { get; set; }
    [Required]
    public int Floors { get; set; }
    [Required]
    public float Margin { get; set; }

    public int? OwnerId { get; set; }
    public virtual Account Owner { get; set; }

    public float Price { get; set; }

    [InverseProperty("BlockOfFlats")]
    public virtual ICollection<Flat> Flats { get; set; } = new List<Flat>();

    [InverseProperty("BlockOfFlatsProperty")]
    public virtual ICollection<Rent> Rents { get; set; } = new List<Rent>();
}
