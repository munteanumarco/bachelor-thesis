namespace DataAccessLayer.Helpers;

public class GetEventsParameters
{
    public int PageSize { get; set; } = 10;
    public int PageNumber { get; set; } = 1;
    
    public string? UserId { get; set; }
}