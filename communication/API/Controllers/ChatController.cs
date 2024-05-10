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
    public async Task<ActionResult<ChatMessagesResponse>> GetChatMessages(Guid chatId, int pageNumber = 1, int pageSize = 10)
    {
        var result = await _chatService.GetChatMessagesAsync(chatId, pageNumber, pageSize);
        if (!result.IsSuccess) return BadRequest(ChatMessagesResponse.Failure(result.Errors));
        return Ok(ChatMessagesResponse.Success(result.Data.Data));
    }
}