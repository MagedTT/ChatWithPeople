using Entities.Enums;
using Microsoft.AspNetCore.Identity;

namespace Entities.Models;

public class User : IdentityUser<Guid>
{
    public UserStatus Status { get; set; } = UserStatus.Offline;
    public byte[]? ProfilePicture { get; set; }
    public DateTime LastSeen { get; set; } = DateTime.Now;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public bool IsDeleted { get; set; } = false;
    public DateTime? DeletedAt { get; set; }
    public int Age { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }

    public ICollection<Friendship> FriendshipsSent { get; set; } = default!;
    public ICollection<Friendship> FriendshipsReceived { get; set; } = default!;
    public ICollection<FriendRequest> SentFriendRequests { get; set; } = default!;
    public ICollection<FriendRequest> ReceivedFriendRequests { get; set; } = default!;

    public ICollection<Message> MessagesSent { get; set; } = default!;
    public ICollection<MessageRead> MessageReads { get; set; } = default!;
    public ICollection<ConversationParticipant> ConversationParticipants { get; set; } = default!;

    public ICollection<Group> GroupsCreated { get; set; } = default!;
    public ICollection<GroupMember> GroupMemberships { get; set; } = default!;
    public ICollection<GroupJoinRequest> GroupJoinRequests { get; set; } = default!;
    public ICollection<GroupInvite> GroupInvitesSent { get; set; } = default!;
    public ICollection<GroupInvite> GroupInvitesReceived { get; set; } = default!;

    public ICollection<Notification> Notifications { get; set; } = default!;

    public ICollection<UserInterest> UserInterests { get; set; } = default!;
}