namespace Shared.DTOs;

public class ConversationParticipantDto
{
    public Guid Id { get; set; }
    public Guid ConversationId { get; set; }
    public Guid UserId { get; set; }
}