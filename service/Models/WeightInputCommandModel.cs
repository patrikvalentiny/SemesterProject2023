using System.ComponentModel.DataAnnotations;

namespace service.Models;

public class WeightInputCommandModel
{
    [Range(20, 500)] public decimal Weight { get; set; }
    public DateTime Date { get; set; }
}