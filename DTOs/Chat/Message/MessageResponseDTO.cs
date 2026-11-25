namespace SigmaMessenger.DTOs.Chat;

public class MessageResponseDTO
{
    public int Id { get; set; }
    public string Text { get; set; }

    public DateTime TimeStamp { get; set; }

    public int SenderId { get; set; }
    public string SenderUsername { get; set; }
}