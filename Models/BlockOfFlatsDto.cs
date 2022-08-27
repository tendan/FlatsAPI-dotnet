namespace FlatsAPI.Models;

public class BlockOfFlatsDto
{
    public int Id { get; set; }

    public string Address { get; set; }
    public string PostalCode { get; set; }
    public int Floors { get; set; }
    public float Margin { get; set; }
    public virtual AccountDto Owner { get; set; }
}
