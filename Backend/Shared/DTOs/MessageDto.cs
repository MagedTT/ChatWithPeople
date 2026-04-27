using Entities.Enums;

namespace Shared.DTOs;

public class MessageDto
{
    public Guid Id { get; set; }
    public Guid SenderId { get; set; }
    public Guid ConversationId { get; set; }
    public string Content { get; set; } = string.Empty;
    public MessageType MessageType { get; set; } = MessageType.Text;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? EditedAt { get; set; }
    public bool IsDeleted { get; set; } = false;
    public IEnumerable<MessageReadDto> MessagesRead { get; set; } = new List<MessageReadDto>();
}