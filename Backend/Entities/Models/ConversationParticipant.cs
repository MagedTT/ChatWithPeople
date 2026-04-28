namespace Entities.Models;

public class ConversationParticipant
{
    public Guid Id { get; set; }

    public Guid ConversationId { get; set; }
    public Conversation Conversation { get; set; } = default!;

    public Guid UserId { get; set; }
    public User User { get; set; } = default!;

    public DateTime LastReadAt { get; set; } = DateTime.Now;
}