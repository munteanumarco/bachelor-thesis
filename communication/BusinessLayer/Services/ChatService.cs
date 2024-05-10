using AutoMapper;
using BusinessLayer.DTOs;
using BusinessLayer.Helpers;
using BusinessLayer.Interfaces;
using DataAccessLayer.Entities;
using DataAccessLayer.Helpers;
using DataAccessLayer.Interfaces;
using Serilog;

namespace BusinessLayer.Services;

public class ChatService : IChatService
{   
    private readonly IChatRepository _chatRepository;
    private readonly IMapper _mapper;
    private readonly Serilog.ILogger _logger;
    public ChatService(IChatRepository chatRepository, IMapper mapper, ILogger logger)
    {
        _chatRepository = chatRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<OperationResult<MessageDto>> SaveMessageAsync(Guid chatId, string userId, string message)
    {
        try
        {

            var newMessage = new Message()
            {
                MessageText = message,
                UserId = userId,
                Date = DateTime.Now,
            };

            var messageToReturn = await _chatRepository.SaveMessageAsync(newMessage);

            var newChatMessage = new ChatMessage()
            {
                ChatId = chatId,
                MessageId = newMessage.Id,
            };

            await _chatRepository.SaveChatMessageAsync(newChatMessage);
            return OperationResult<MessageDto>.Success(_mapper.Map<MessageDto>(messageToReturn));
        }
        catch (Exception ex)
        {
           _logger.Error($"Error occurred while saving message: {ex.Message}");
           return OperationResult<MessageDto>.Failure(new List<string>(){"Error occurred while saving message."});
        }
    }
    
    public async Task<OperationResult<IEnumerable<Guid>>> GetUserChatIds(string userId)
    {
        try
        {
            var chatIds = await _chatRepository.GetUserChatIds(userId);
            return OperationResult<IEnumerable<Guid>>.Success(chatIds);
        }
        catch (Exception ex)
        {
            _logger.Error($"Error occurred while getting user chat ids: {ex.Message}");
            return OperationResult<IEnumerable<Guid>>.Failure(new List<string>(){"Error occurred while getting user chat ids."});
        }
    }

    public async Task<OperationResult<PagedResult<MessageDto>>> GetChatMessagesAsync(Guid chatId, int pageNumber, int pageSize)
    {
        try
        {
            var messages = await _chatRepository.GetChatMessagesAsync(chatId, pageNumber, pageSize);
            return OperationResult<PagedResult<MessageDto>>.Success(_mapper.Map<PagedResult<MessageDto>>(messages));
        }
        catch (Exception e)
        {
            _logger.Error($"Error occurred while getting chat messages: {e.Message}");
            return OperationResult<PagedResult<MessageDto>>.Failure(new List<string>(){"Error occurred while getting chat messages."});
        }
    }
}