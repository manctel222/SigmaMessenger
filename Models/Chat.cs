namespace SigmaMessenger.Models;

public class Chat
{
    public int Id { get; set; }
    
    public string? Title { get; set; }
    
    public bool isGroup { get; set; }
    
    public DateTime Timestamp { get; set; }
    
    public List<ChatMember> Members { get; set; }
    public List<Message> Messages { get; set; }
}