using System.ComponentModel.DataAnnotations;

namespace api.Dtos;

public class WeightDto
{
    [Required] [Range(20, 500)] public decimal Weight { get; set; }
    [Required] public DateTime Date { get; set; }
    public decimal? Difference { get; set; }
    public float? BodyFatPercentage { get; set; }
    public float? SkeletalMuscleWeight { get; set; }
}