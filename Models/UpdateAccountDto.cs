namespace FlatsAPI.Models;

public class UpdateAccountDto
{
    public string Email { get; set; }
    public string Username { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
    public string BillingAddress { get; set; }
    public string PhoneNumber { get; set; }
}
