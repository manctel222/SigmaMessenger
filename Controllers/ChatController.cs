using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SigmaMessenger.DTOs.Chat;
using SigmaMessenger.Services;

namespace SigmaMessenger.Controllers;
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ChatController : ControllerBase
{
    private readonly ChatService _chatService;

    public ChatController(ChatService chatService)
    {
        _chatService = chatService;
    }

    [HttpPost("private")]
    public async Task<IActionResult> CreatePrivateChat(int targetUserId)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!int.TryParse(userIdClaim, out int currentUserId))
        {
            return Unauthorized("Некорректный токен.");
        }
        try
        {
            var chat = await _chatService.CreatePrivateChatAsync(currentUserId, targetUserId);
            return Ok(chat);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetMyChats()
    {
        var userIdClaim = User.FindFirstValue((ClaimTypes.NameIdentifier));
        if (!int.TryParse(userIdClaim, out int currentUserId))
        {
            return Unauthorized("Некорректный токен");
        }
        var myChats = await _chatService.GetUserChatsAsync(currentUserId);
        return Ok(myChats);
    }
}