namespace SigmaMessenger.Models;

public class ChatMember
{
    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public int ChatId { get; set; }
    public Chat Chat { get; set; } = null!;

    public DateTime JoinedAt { get; set; }

    public string Role { get; set; }

    public int LastReadMessageId { get; set; }
}