using System.ComponentModel.DataAnnotations;

namespace infrastructure.DataModels;

public class UserDetails
{
    public int? UserId { get; set; }
    public string? Firstname { get; set; }
    public string? Lastname { get; set; }
    [Required] public required int Height { get; set; }
    [Required] public required decimal TargetWeight { get; set; }
    public DateTime? TargetDate { get; set; }
    public decimal? LossPerWeek { get; set; }
}