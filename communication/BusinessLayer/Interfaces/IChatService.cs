using BusinessLayer.DTOs;
using BusinessLayer.Helpers;
using DataAccessLayer.Entities;
using DataAccessLayer.Helpers;

namespace BusinessLayer.Interfaces;

public interface IChatService
{
    Task<OperationResult<MessageDto>> SaveMessageAsync(Guid chatId, string userId, string message);
    Task<OperationResult<IEnumerable<Guid>>> GetUserChatIds(string userId);
    Task<OperationResult<PagedResult<MessageDto>>> GetChatMessagesAsync(Guid chatId, int pageNumber, int pageSize);
}