namespace Application.Shared.Models.Request;

public class OpeningHoursRequest
{
    public DayOfWeek DayOfWeek { get; set; }
    public DateTime OpeningTime { get; set; }
    public DateTime ClosingTime { get; set; }
        
    public DateTime? OpeningTime2 { get; set; }
    public DateTime? ClosingTime2 { get; set; }
}