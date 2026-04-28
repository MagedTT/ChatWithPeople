using Entities.Enums;

namespace Entities.Models;

public class Message
{
    public Guid Id { get; set; }

    public Guid SenderId { get; set; }
    public User Sender { get; set; } = default!;

    public Guid ConversationId { get; set; }
    public Conversation Conversation { get; set; } = default!;

    public string Content { get; set; } = string.Empty;
    public MessageType MessageType { get; set; } = MessageType.Text;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? EditedAt { get; set; }
    public bool IsDeleted { get; set; } = false;
}