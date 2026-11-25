namespace SigmaMessenger.Models;

public class Message
{
    public int Id { get; set; }

    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public int ChatId { get; set; }
    public Chat Chat { get; set; } = null!;

    public string Text { get; set; }

    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}