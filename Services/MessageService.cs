using Microsoft.AspNetCore.Mvc.TagHelpers.Cache;
using Microsoft.AspNetCore.SignalR;
using SigmaMessenger.Hubs;
using SigmaMessenger.Data;
using SigmaMessenger.Models;
using SigmaMessenger.DTOs.Chat.Message;
using Microsoft.EntityFrameworkCore;
using SigmaMessenger.DTOs.Chat;

namespace SigmaMessenger.Services;

public class MessageService
{
    private readonly AppDbContext _db;
    private readonly IHubContext<ChatHub> _hubContext;

    public MessageService(AppDbContext db, IHubContext<ChatHub> hubContext)
    {
        _db = db;
        _hubContext = hubContext;
    }

    public async Task<MessageResponseDTO> SaveMessageAsync(int senderId, MessageDTO dto)
    {
        if (!await _db.ChatMembers.AnyAsync(cm =>
                cm.ChatId == dto.ChatId &&
                cm.UserId == senderId))
            throw new UnauthorizedAccessException("Вы не состоите в этом чате.");
        var message = new Message
        {
            ChatId = dto.ChatId,
            UserId = senderId,
            Text = dto.Text,
            Timestamp = DateTime.UtcNow
        };
        _db.Messages.Add(message);
        await _db.SaveChangesAsync();
        var sender = await _db.Users.FindAsync(senderId);
        var responseDTO = new MessageResponseDTO
        {
            Id = message.Id,
            Text = message.Text,
            TimeStamp = message.Timestamp,
            SenderUsername = sender?.Username ?? "Unknown",
            SenderId = message.UserId
        };
        await _hubContext.Clients
            .Group($"chat_{dto.ChatId}")
            .SendAsync("ReceiveMessage", responseDTO);
        return responseDTO;
    }

    public async Task<List<MessageResponseDTO>> GetChatMessagesAsync(int userId, int chatId)
    {
        if (!await _db.ChatMembers.AnyAsync(cm =>
                cm.ChatId == chatId &&
                cm.UserId == userId))
            throw new UnauthorizedAccessException("Вы не состоите в этом чате.");
        var messages = await _db.Messages
            .Include(m => m.User)
            .Where(m => m.ChatId == chatId)
            .OrderByDescending(m => m.Timestamp)
            .Take(50)
            .ToListAsync();
        return messages.Select(m => new MessageResponseDTO
        {
            Id = m.Id,
            Text = m.Text,
            TimeStamp = m.Timestamp,
            SenderUsername = m.User.Username,
            SenderId = m.UserId
        }).ToList();
    }
}