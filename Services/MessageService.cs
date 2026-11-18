using SigmaMessenger.Data;
using SigmaMessenger.Models;
using Microsoft.EntityFrameworkCore;

namespace SigmaMessenger.Services;

public class MessageService
{
    private readonly AppDbContext _db;
    public MessageService(AppDbContext db)
    {
        _db = db;
    }

    public async Task SaveMessageAsync(Message message)
    {
        _db.Messages.Add(message);
        await _db.SaveChangesAsync();
    }

    public async Task<List<Message>> GetAllMessagesAsync()
    {
        return await _db.Messages.ToListAsync();    
    }
}