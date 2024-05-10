namespace DataAccessLayer.Entities;

public class Participant
{
    public Guid Id { get; set; }
    public Guid EmergencyEventId { get; set; }
    public string UserId { get; set; }
    public EmergencyEvent Event { get; set; }
    public EmergencyAppUser User { get; set; }
}