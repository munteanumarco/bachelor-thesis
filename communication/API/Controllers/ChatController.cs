using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
[ApiController]
[Route("api/chats")]
public class ChatController : ControllerBase
{
    public ChatController()
    {
    }
    
    [HttpGet]
    public async Task<ActionResult> GetChatsAsync()
    {
        return Ok();
    }
}