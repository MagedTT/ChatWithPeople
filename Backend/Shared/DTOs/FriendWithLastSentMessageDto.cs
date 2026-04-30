using Entities.Enums;

namespace Shared.DTOs;

public class FriendWithLastSentMessageDto
{
    public Guid FriendId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string? ProfilePicture { get; set; }
    public DateTime LastActive { get; set; }
    public UserStatus UserStatus { get; set; }
    public string? LastSentMessage { get; set; }
}