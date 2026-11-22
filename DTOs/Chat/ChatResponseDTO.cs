using SigmaMessenger.Models;

namespace SigmaMessenger.DTOs.Chat;

public class ChatResponseDTO
{
    public int ChatId { get; set; }
    public string ChatTitle { get; set; }
    
    public bool IsGroupChat { get; set; }
    
    public List<ChatMember> ChatMembers;
}