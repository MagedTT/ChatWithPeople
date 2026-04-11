using Entities.Enums;

namespace Entities.Models;

public class FriendRequest
{
    public Guid Id { get; set; }

    public Guid SenderId { get; set; }
    public User Sender { get; set; } = default!;

    public Guid ReceiverId { get; set; }
    public User Receiver { get; set; } = default!;

    public FriendRequestStatus FriendRequestStatus { get; set; } = FriendRequestStatus.Pending;

    public DateTime CreatedAt { get; set; } = DateTime.Now;
}