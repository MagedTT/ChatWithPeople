using Entities.Enums;

namespace Entities.Models;

public class Friend
{
    public Guid FriendId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string? ProfilePicture { get; set; }
    public UserStatus UserStatus { get; set; }
}