// using BusinessLayer.Interfaces;
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.SignalR;
//
// namespace API.Hubs;
//
// [Authorize]
// public class ChatHub : Hub
// {
//     private readonly IChatService _chatService;
//         
//     public ChatHub(IChatService chatService) 
//     {
//         _chatService = chatService;
//     }
//
//     public async Task<Task> SendMessageToChatGroup(string chatId, string message)
//     {
//         var userId = Context.User?.FindFirst(SolutionConfigurationConstants.JwtIdClaim)?.Value;
//         var savedMessage = await _chatService.SaveMessageAsync(new Guid(chatId), userId, message);    
//         return Clients.Group(chatId.ToUpper()).SendAsync("ReceiveMessage", savedMessage);
//     }
//     public override async Task OnConnectedAsync()
//     {
//         var userId = Context.User?.FindFirst(SolutionConfigurationConstants.JwtIdClaim)?.Value;
//
//         if (userId.IsNullOrEmpty())
//         {
//             await Clients.Caller.SendAsync("UnauthorizedAccess", "You are not authorized.");
//             await Task.Delay(500);
//             Context.Abort();
//         }
//
//         var chats = await _chatService.GetUserChatIds(userId);
//         foreach (var chatId in chats)
//         {
//             Console.WriteLine($"user with id {userId} is in chat with id {chatId}");
//             await Groups.AddToGroupAsync(Context.ConnectionId, chatId.ToString().ToUpper());
//         }
//
//         await base.OnConnectedAsync();
//     }
//
//     public override async Task OnDisconnectedAsync(Exception exception)
//     {
//         var userId = Context.User?.FindFirst(SolutionConfigurationConstants.JwtIdClaim)?.Value;
//         var chats = await _chatService.GetUserChatIds(userId);
//
//         foreach (var chatId in chats)
//         {
//             await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatId.ToString().ToUpper());
//         }
//
//         await base.OnDisconnectedAsync(exception);
//     }
// }
// }