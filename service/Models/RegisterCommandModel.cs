using System.ComponentModel.DataAnnotations;

namespace service.Models;

public class RegisterCommandModel
{
    [Required] [MinLength(4)] public required string Password { get; set; }
    [Required] public required string Username { get; set; }
    [Required] [EmailAddress] public required string Email { get; set; }
}