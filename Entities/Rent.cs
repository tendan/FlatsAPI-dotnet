using FlatsAPI.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;



namespace FlatsAPI.Entities;

public enum PropertyTypes
{
    Flat,
    BlockOfFlats
}
public class Rent
{
    public int Id { get; set; }

    [Required]
    public DateTime CreationDate { get; set; }
    [Required]
    public DateTime PayDate { get; set; }
    [Required]
    [Column("Paid")]
    public bool Paid { get; set; }

    [Required]
    [Column("Price")]
    public float Price { get; set; }

    [Required]
    [Column("OwnerShip")]
    public OwnerShip OwnerShip { get; set; }

    [Required]
    [Column("PriceWithTax")]
    public float PriceWithTax { get; set; }

    [Required]
    [Column("RentIssuerId")]
    public int RentIssuerId { get; set; }
    public virtual Account RentIssuer { get; set; }

    [Column("PropertyId")]
    public int PropertyId { get; set; }
    public virtual BlockOfFlats BlockOfFlatsProperty { get; set; }
    public virtual Flat FlatProperty { get; set; }
    public virtual PropertyTypes PropertyType { get; set; }
}
