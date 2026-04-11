using Entities.Enums;

namespace Entities.Models;

public class GroupMember
{
    public Guid Id { get; set; }

    public Guid GroupId { get; set; }
    public Group Group { get; set; } = default!;

    public Guid UserId { get; set; }
    public User User { get; set; } = default!;

    public GroupMemberRole MemberRole { get; set; } = GroupMemberRole.Member;
    public DateTime JoinedAt { get; set; } = DateTime.Now;
}