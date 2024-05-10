using DataAccessLayer.Entities;
using DataAccessLayer.Helpers;

namespace DataAccessLayer.Interfaces;

public interface IChatRepository
{
    Task<Message?> SaveMessageAsync(Message newMessage);
    Task<IEnumerable<Guid>> GetUserChatIds(string userId);
    Task<string> SaveChatMessageAsync(ChatMessage newChatMessage);
    Task<PagedResult<Message>> GetChatMessagesAsync(Guid chatId, int pageNumber, int pageSize);
}