using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SigmaMessenger.Data;
using SigmaMessenger.Models;

namespace SigmaMessenger.Hubs;

[Authorize]
public class ChatHub : Hub
{
    private readonly AppDbContext _db;

    public ChatHub(AppDbContext db)
    {
        _db = db;
    }

    public async Task JoinChat(int chatId)
    {
        var userId = Context.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!int.TryParse(userId, out int currentUserId))
        {
            throw new HubException("Некорректный токен.");
        }

        bool isMember = await _db.ChatMembers
            .AnyAsync(cm => cm.ChatId == chatId && cm.UserId == currentUserId);
        if (!isMember)
        {
            throw new HubException("Нет доступа к этому чату.");
        }

        await Groups.AddToGroupAsync(Context.ConnectionId, $"chat_{chatId}");
    }

    public async Task LeaveChat(int chatId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"chat_{chatId}");
    }
}