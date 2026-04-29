using Entities.Enums;

namespace Shared.DTOs;

public class FriendDto
{
    public Guid FriendId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string? ProfilePicture { get; set; }
    public UserStatus UserStatus { get; set; }
}