using Microsoft.EntityFrameworkCore;
using SigmaMessenger.Data;
using SigmaMessenger.DTOs.Chat;
using SigmaMessenger.Models;

namespace SigmaMessenger.Services;

public class ChatService
{
    private readonly AppDbContext _db;

    public ChatService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<ChatResponseDTO> CreatePrivateChatAsync(int currentUserId, int targetUserId)
    {
        if (currentUserId == targetUserId)
        {
            throw new ArgumentException("Нельзя создать чат с самим собой.");
        }

        var targetUser = await _db.Users.FindAsync(targetUserId);
        if (targetUser == null)
        {
            throw new KeyNotFoundException("Пользователь не найден.");
        }

        var existingChat = await _db.Chats
            .Include(c => c.Members)
            .ThenInclude(m => m.User)
            .FirstOrDefaultAsync(c =>
                !c.isGroup &&
                c.Members.Any(m => m.UserId == currentUserId) &&
                c.Members.Any(m => m.UserId == targetUserId));
        if (existingChat != null)
        {
            return new ChatResponseDTO
            {
                ChatId = existingChat.Id,
                ChatTitle = targetUser.Username
            };
        }

        var newChat = new Chat { isGroup = false };
        _db.Chats.Add(newChat);
        await _db.SaveChangesAsync();
        _db.ChatMembers.AddRange(
            new ChatMember { ChatId = newChat.Id, UserId = currentUserId, Role = "Member" },
            new ChatMember { ChatId = newChat.Id, UserId = targetUserId, Role = "Member" });
        await _db.SaveChangesAsync();
        return new ChatResponseDTO
        {
            ChatId = newChat.Id,
            ChatTitle = targetUser.Username
        };
    }

    public async Task<List<ChatResponseDTO>> GetUserChatsAsync(int currentUserId)
    {
        var list = await _db.Chats
            .Include(c => c.Members)
            .ThenInclude(m => m.User)
            .Where(c => c.Members.Any(m => m.UserId == currentUserId))
            .ToListAsync();
        return list.Select(c => new ChatResponseDTO
        {
            ChatId = c.Id,
            ChatTitle = c.isGroup
                ? c.Title
                : c.Members.FirstOrDefault(m => m.UserId != currentUserId)
                    ?.User.Username ?? "Чат"
        }).ToList();
    }
}