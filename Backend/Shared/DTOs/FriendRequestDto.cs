using Entities.Enums;

namespace Shared.DTOs;

public class FriendRequestDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string? ProfilePicture { get; set; }
    public FriendRequestStatus FriendRequestStatus { get; set; }
    public DateTime CreatedAt { get; set; }
}