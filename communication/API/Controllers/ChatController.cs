using API.Responses;
using BusinessLayer.DTOs;
using BusinessLayer.Interfaces;
using DataAccessLayer.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
[ApiController]
[Route("api/chats")]
public class ChatController : ControllerBase
{
    private readonly IChatService _chatService;
    public ChatController(IChatService chatService)
    {
        _chatService = chatService;
    }

    [HttpGet("{chatId}/messages")]
    public async Task<ActionResult<ChatMessagesResponse>> GetChatMessages(Guid chatId)
    {
        var result = await _chatService.GetChatMessagesAsync(chatId);
        if (!result.IsSuccess) return BadRequest(ChatMessagesResponse.Failure(result.Errors));
        return Ok(ChatMessagesResponse.Success(result.Data));
    }
    
    [HttpGet("{eventId}/details")]
    public async Task<ActionResult<GetChatDetailsResponse>> GetChatDetails(Guid eventId)
    {
        var result = await _chatService.GetChatDetailsAsync(eventId);
        if (!result.IsSuccess) return BadRequest(GetChatDetailsResponse.Failure(result.Errors));
        return Ok(GetChatDetailsResponse.Success(result.Data));
    }
}