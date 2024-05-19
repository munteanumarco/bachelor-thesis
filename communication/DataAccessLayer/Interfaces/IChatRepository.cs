using DataAccessLayer.Entities;
using DataAccessLayer.Helpers;

namespace DataAccessLayer.Interfaces;

public interface IChatRepository
{
    Task<Message?> SaveMessageAsync(Message newMessage);
    Task<IEnumerable<Guid>> GetUserChatIds(string userId);
    Task<string> SaveChatMessageAsync(ChatMessage newChatMessage);
    Task<IEnumerable<Message>> GetChatMessagesAsync(Guid chatId);
    Task AddChatEventAsync(Guid emergencyEventId, string name);
    Task<ChatEvent?> GetChatDetailsAsync(Guid eventId);
    Task<int> GetParticipantCountAsync(Guid eventId);
}