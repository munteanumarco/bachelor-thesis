using BusinessLayer.DTOs;

namespace API.Responses;

public class GetMessagesResponse
{
    public IEnumerable<MessageDto?> Messages { get; set; }
    public Guid ChatId { get; set; }
    public int TotalCount { get; set; }
}