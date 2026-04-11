namespace Entities.Models;

public class MessageRead
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }
    public User User { get; set; } = default!;

    public Guid MessageId { get; set; }
    public Message Message { get; set; } = default!;

    public DateTime ReadAt { get; set; } = DateTime.Now;
}