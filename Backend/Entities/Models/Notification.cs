using Entities.Enums;

namespace Entities.Models;

public class Notification
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }
    public User User { get; set; } = default!;

    public NotificationType NotificationType { get; set; } = NotificationType.FriendRequest;
    public Guid ReferenceId { get; set; } // can be FriendRequest, Message, GroupInvite, or JoinRequest

    public string Content { get; set; } = string.Empty;
    public bool IsRead { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}