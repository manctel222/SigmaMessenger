using Microsoft.AspNetCore.Mvc;
using SigmaMessenger.Models;
using SigmaMessenger.Services;

namespace SigmaMessenger.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MessageController : ControllerBase
{
    private readonly MessageService _ms;
    public MessageController(MessageService ms)
    {
        _ms = ms;
    }
    [HttpPost]
    public async Task<IActionResult> SendMessage(Message message)
    {
        await _ms.SaveMessageAsync(message);
        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> GetMessages()
    {
        var list = await _ms.GetAllMessagesAsync();
        return Ok(list);
    }
}