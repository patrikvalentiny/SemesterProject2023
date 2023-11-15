using System.ComponentModel.DataAnnotations;

namespace infrastructure.DataModels;

public class WeightInput
{
    public int Id { get; set; }
    [Required] [Range(20, maximum:500)] public decimal Weight { get; set; }
    [Required] public DateTime Date { get; set; }
    [Required] public int UserId { get; set; }
}