using Entities.Enums;

namespace Shared.DTOs;

public class UserForDiscoverDTO
{
    public Guid Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string? ProfilePicture { get; set; }
    public IEnumerable<string> Interests { get; set; } = new List<string>();
    public UserStatus Status { get; set; }
    public int Age { get; set; }
}