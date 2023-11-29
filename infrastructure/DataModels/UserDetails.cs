namespace infrastructure.DataModels;

public class UserDetails
{
    public int? UserId { get; set; }
    public string? Firstname { get; set; }
    public string? Lastname { get; set; }
    public int? Height { get; set; }
    public decimal? TargetWeight { get; set; }
    public DateTime? TargetDate { get; set; }
    public decimal? LossPerWeek { get; set; }
}