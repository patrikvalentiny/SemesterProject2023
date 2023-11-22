using System.ComponentModel.DataAnnotations;

namespace api.Dtos;

public class WeightDto
{
    [Required] public int Id { get; set; }
    [Required] [Range(20, maximum:500)] public decimal Weight { get; set; }
    [Required] public DateTime Date { get; set; }
}