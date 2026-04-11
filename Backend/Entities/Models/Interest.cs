namespace Entities.Models;

public class Interest
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public ICollection<UserInterest> UserInterests { get; set; } = default!;
}