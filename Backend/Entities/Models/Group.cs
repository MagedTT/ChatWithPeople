namespace Entities.Models;

public class Group
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Guid CreatedByUserId { get; set; }
    public User CreatedBy { get; set; } = default!;
    public bool IsPublic { get; set; } = true;
    public byte[]? Avatar { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public Conversation Conversation { get; set; } = default!;

    public ICollection<GroupMember> Members { get; set; } = default!;
    public ICollection<GroupJoinRequest> JoinRequests { get; set; } = default!;
    public ICollection<GroupInvite> Invites { get; set; } = default!;
}