using BusinessLayer.DTOs;
using BusinessLayer.Helpers;
using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Tokens;
using ILogger = Serilog.ILogger;

namespace API.Hubs;

[Authorize]
public class ChatHub : Hub
{
    private readonly IChatService _chatService;
    private readonly ILogger _logger;
    public ChatHub(IChatService chatService, ILogger logger)
    {
        _chatService = chatService;
        _logger = logger;
    }

    public async Task<Task> SendMessageToChatGroup(string chatId, string message)
    {
        var userId = Context.User?.FindFirst(CustomClaimTypes.UserId)?.Value;
        _logger.Information($"Sending message to chat with id {chatId} from user with id {userId}");
        var result = await _chatService.SaveMessageAsync(new Guid(chatId), userId, message);
        var messageDto = new EnhancedMessageDto()
        {
            Id = result.Data.Id,
            Text = result.Data.MessageText,
            Date = result.Data.Date,
            Username = result.Data.Username
        };
        
        return Clients.Group(chatId.ToUpper()).SendAsync("ReceiveMessage", messageDto);
    }
    public override async Task OnConnectedAsync()
    {
        var userId = Context.User?.FindFirst(CustomClaimTypes.UserId)?.Value;

        if (userId.IsNullOrEmpty())
        {
            await Clients.Caller.SendAsync("UnauthorizedAccess", "You are not authorized.");
            await Task.Delay(500);
            Context.Abort();
        }
        
        var result = await _chatService.GetUserChatIds(userId);
        if (!result.IsSuccess) return;

        var chats = result.Data;
        
        foreach (var chatId in chats)
        {
            _logger.Information($"user with id {userId} is in chat with id {chatId}");
            await Groups.AddToGroupAsync(Context.ConnectionId, chatId.ToString().ToUpper());
        }

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        var userId = Context.User?.FindFirst(CustomClaimTypes.UserId)?.Value;
        var result = await _chatService.GetUserChatIds(userId);
        
        if (!result.IsSuccess) return;

        var chats = result.Data;
        
        foreach (var chatId in chats)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatId.ToString().ToUpper());
        }

        await base.OnDisconnectedAsync(exception);
    }
}
