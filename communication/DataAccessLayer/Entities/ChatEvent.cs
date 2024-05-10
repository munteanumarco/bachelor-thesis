namespace DataAccessLayer.Entities;

public class ChatEvent
{
    public Guid Id { get; set; }
    public Guid EmergencyEventId { get; set; }
    public string Name { get; set; }
    public EmergencyEvent EmergencyEvent { get; set; }
    public ICollection<ChatMessage> ChatMessages { get; set; }
}