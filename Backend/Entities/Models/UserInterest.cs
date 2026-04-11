namespace Entities.Models;

public class UserInterest
{
    public Guid UserId { get; set; }
    public User User { get; set; } = default!;

    public Guid InterestId { get; set; }
    public Interest Interest { get; set; } = default!;
}