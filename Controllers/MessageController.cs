using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SigmaMessenger.Models;
using SigmaMessenger.Services;
using SigmaMessenger.DTOs.Chat.Message;

namespace SigmaMessenger.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class MessageController : ControllerBase
{
    private readonly MessageService _ms;

    public MessageController(MessageService ms)
    {
        _ms = ms;
    }

    [HttpPost]
    public async Task<IActionResult> SendMessage(MessageDTO dto)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!int.TryParse(userIdClaim, out int currentUserId))
        {
            return Unauthorized("Некорректный токен.");
        }

        try
        {
            var message = await _ms.SaveMessageAsync(currentUserId, dto);
            return Ok(message);
        }
        catch (UnauthorizedAccessException ex)
        {
            return StatusCode(403, ex.Message);
        }
    }

    [HttpGet("{chatId}")]
    public async Task<IActionResult> GetMessages(int chatId)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!int.TryParse(userIdClaim, out int currentUserId))
        {
            return Unauthorized("Некорректный токен.");
        }

        try
        {
            var messages = await _ms.GetChatMessagesAsync(currentUserId, chatId);
            return Ok(messages);
        }
        catch (UnauthorizedAccessException ex)
        {
            return StatusCode(403, ex.Message);
        }
    }
}