using System.ComponentModel.DataAnnotations;

namespace infrastructure.DataModels;

public class WeightInput
{
    [Required] [Range(20, 500)] public required decimal Weight { get; set; }
    [Required] public required DateTime Date { get; set; }
    [Required] public required int UserId { get; set; }
    public decimal? Difference { get; set; }
    public float? BodyFatPercentage { get; set; }
    public float? SkeletalMuscleWeight { get; set; }
}