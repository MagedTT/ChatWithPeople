using Entities.Enums;

namespace Shared.DTOs;

public class ConversationDto
{
    public Guid Id { get; set; }
    public ConversationType Type { get; set; }
    public DateTime CreatedAt { get; set; }
    public IEnumerable<MessageDto> Messages { get; set; } = new List<MessageDto>();
    public IEnumerable<ConversationParticipantDto> ConversationParticipants { get; set; } = new List<ConversationParticipantDto>();
}