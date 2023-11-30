using System.ComponentModel.DataAnnotations;

namespace service.Models;

public class LoginCommandModel
{
    [Required] public required string Username { get; init; }
    [Required] [MinLength(4)] public required string Password { get; init; }
}