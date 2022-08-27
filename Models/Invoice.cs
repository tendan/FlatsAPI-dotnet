namespace FlatsAPI.Models;

public class Invoice
{
    public byte[] FileContents { get; set; }
    public string ContentType { get; set; }
    public string FileName { get; set; }
}
