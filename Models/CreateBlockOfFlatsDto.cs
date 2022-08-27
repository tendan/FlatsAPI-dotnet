namespace FlatsAPI.Models;

public class CreateBlockOfFlatsDto
{
    public string Address { get; set; }
    public string PostalCode { get; set; }
    public int Floors { get; set; }
    public float Margin { get; set; }
    public int? OwnerId { get; set; }
}
