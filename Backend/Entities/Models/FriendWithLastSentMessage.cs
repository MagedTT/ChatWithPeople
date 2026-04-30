using Entities.Enums;

namespace Entities.Models;

public class FriendWithLastSentMessage
{
    public Guid FriendId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string? ProfilePicture { get; set; }
    public DateTime LastActive { get; set; }
    public UserStatus UserStatus { get; set; }
    public string? LastSentMessage { get; set; }
}