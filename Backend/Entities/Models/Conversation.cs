using Entities.Enums;

namespace Entities.Models;

public class Conversation
{
    public Guid Id { get; set; }
    public ConversationType Type { get; set; } = ConversationType.Private;

    public Guid? GroupId { get; set; }
    public Group? Group { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public ICollection<Message> Messages { get; set; } = default!;
    public ICollection<ConversationParticipant> ConversationParticipants { get; set; } = default!;
}