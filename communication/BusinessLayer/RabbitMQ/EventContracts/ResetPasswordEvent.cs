namespace BusinessLayer.RabbitMQ.EventContracts;

public class ResetPasswordEvent
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string ResetLink { get; set; }
}