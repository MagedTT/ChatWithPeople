using Entities.Enums;
using Entities.Models;

namespace Shared.DTOs;

public class FriendRequestDto
{
    public Guid Id { get; set; }
    public Guid SenderId { get; set; }
    public UserDto Sender { get; set; } = default!;

    public Guid ReceiverId { get; set; }
    public UserDto Receiver { get; set; } = default!;

    public FriendRequestStatus FriendRequestStatus { get; set; }
    public DateTime CreatedAt { get; set; }
}