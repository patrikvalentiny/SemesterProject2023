using System.ComponentModel.DataAnnotations;

namespace infrastructure.DataModels;

public class WeightInput
{
    [Required] [Range(20, 500)] public decimal Weight { get; set; }
    [Required] public DateTime Date { get; set; }
    [Required] public int UserId { get; set; }
}