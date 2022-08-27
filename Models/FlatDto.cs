namespace FlatsAPI.Models;

public class FlatDto : FlatInBlockOfFlatsDto
{
    public virtual BlockOfFlatsDto BlockOfFlats { get; set; }
}
