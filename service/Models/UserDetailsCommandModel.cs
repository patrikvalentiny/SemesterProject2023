using System.ComponentModel.DataAnnotations;

namespace service.Models;

public class UserDetailsCommandModel
{
    public string? Firstname { get; set; }
    public string? Lastname { get; set; }
    [Required] [Range(150, 250)] public required int Height { get; set; }
    [Required] [Range(30, 350)] public required decimal TargetWeight { get; set; }
    [Required] public DateTime? TargetDate { get; set; }
    public decimal? LossPerWeek { get; set; }
}