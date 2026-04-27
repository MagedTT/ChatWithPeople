namespace Shared.DTOs;

public class MessageReadDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid MessageId { get; set; }
    public DateTime ReadAt { get; set; }
}