namespace Shared.DTOs;

public class FriendshipsDto
{
    public Guid Id { get; set; }

    public Guid User1Id { get; set; }

    public Guid User2Id { get; set; }
    public UserDto User2 { get; set; } = default!;

    public DateTime CreatedAt { get; set; }
}