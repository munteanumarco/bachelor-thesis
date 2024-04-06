namespace BusinessLayer.RabbitMQ.EventContracts;

public class UserCreatedEvent
{
    public string Email { get; set; }
    public string Username { get; set; }
    public string ConfirmationLink { get; set; }
}