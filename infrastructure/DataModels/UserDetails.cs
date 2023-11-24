namespace infrastructure.DataModels;

public class UserDetails
{
    public int? UserId { get; set; }
    public int? Height { get; set; }
    public decimal? TargetWeight { get; set; }
    public DateTime? TargetDate { get; set; }
}