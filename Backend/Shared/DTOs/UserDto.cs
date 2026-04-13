using Entities.Enums;

namespace Shared.DTOs;

public class UserDto
{
    public Guid Id { get; set; }
    public UserStatus Status { get; set; }
    public byte[]? ProfilePicture { get; set; }
    public DateTime LastSeen { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
    public int Age { get; set; }
}