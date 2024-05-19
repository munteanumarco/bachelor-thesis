namespace BusinessLayer.DTOs;

public class MessageDto
{
    public Guid Id { get; set; }
    public string Username { get; set; }
    public string UserId { get; set; }
    public Guid ChatId { get; set; }    
    public string MessageText { get; set;}
    public DateTime Date { get; set; }
}