using Entities.Enums;

namespace Shared.DTOs;

public class GroupDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public Guid CreatedByUserId { get; set; }
    public bool IsPublic { get; set; }
    public byte[]? Avatar { get; set; }
    public DateTime CreatedAt { get; set; }
    public int MembersCount { get; set; }
}