using Microsoft.AspNetCore.Http;

namespace Shared.DTOs;

public class GroupForCreationDto
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public Guid CreatedByUserId { get; set; }
    public bool IsPublic { get; set; }
    public IFormFile? Avatar { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    // public ICollection<GroupMemberDto>? MembersDtos { get; set; }
}