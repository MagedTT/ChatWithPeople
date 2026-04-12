using Microsoft.AspNetCore.Http;

namespace Shared.DTOs;

public class UserForRegisterationDto
{
    public required string UserName { get; set; }
    public required string Password { get; set; }
    public required string Email { get; set; }
    public string? PhoneNumber { get; set; }
    public ICollection<string>? Roles { get; set; }
    public IFormFile? ProfilePicture { get; set; }
    public int Age { get; set; }
    public ICollection<Guid> InterestIds { get; set; } = new List<Guid>();
}