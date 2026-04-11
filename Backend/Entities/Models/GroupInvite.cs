using Entities.Enums;

namespace Entities.Models;

public class GroupInvite
{
    public Guid Id { get; set; }

    public Guid GroupId { get; set; }
    public Group Group { get; set; } = default!;

    public Guid InvitedUserId { get; set; }
    public User InvitedUser { get; set; } = default!;

    public Guid InvitedByUserId { get; set; }
    public User InvitedByUser { get; set; } = default!;

    public GroupInviteStatus InviteStatus { get; set; } = GroupInviteStatus.Pending;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}