namespace service.Models;

public class LoginCommandModel
{
    public required string Username { get; set; }
    public required string Password { get; set; }
    public string? Email { get; set; }
}