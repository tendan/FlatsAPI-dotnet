namespace FlatsAPI.Models;

public class FlatInRentDto
{
    public int Id { get; set; }
    public int Area { get; set; }
    public int Number { get; set; }
    public int NumberOfRooms { get; set; }
    public int Floor { get; set; }

    public OwnerShip? OwnerShip { get; set; }

    public float PriceWhenBought { get; set; }
    public float? PricePerMeterSquaredWhenRented { get; set; }
}
