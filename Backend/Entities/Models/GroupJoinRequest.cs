using Entities.Enums;

namespace Entities.Models;

public class GroupJoinRequest
{
    public Guid Id { get; set; }

    public Guid GroupId { get; set; }
    public Group Group { get; set; } = default!;

    public Guid UserId { get; set; }
    public User User { get; set; } = default!;

    public GroupJoinRequestStatus JoinRequestStatus { get; set; } = GroupJoinRequestStatus.Pending;

    public DateTime CreatedAt { get; set; } = DateTime.Now;
}