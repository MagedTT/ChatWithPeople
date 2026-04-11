namespace Entities.Models;

public class Friendship
{
    public Guid Id { get; set; }

    public Guid User1Id { get; set; }
    public User User1 { get; set; } = default!;

    public Guid User2Id { get; set; }
    public User User2 { get; set; } = default!;

    public DateTime CreatedAt { get; set; } = DateTime.Now;
}