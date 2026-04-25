using Entities.Enums;

namespace Shared.DTOs;

public class GroupMemberDto
{
    public Guid UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public GroupMemberRole MemberRole { get; set; }
    public DateTime JoinedAt { get; set; }
}