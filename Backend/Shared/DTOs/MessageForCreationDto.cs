namespace Shared.DTOs;

public class MessageForCreationDto
{
    public Guid SenderId { get; set; }
    public string Content { get; set; } = string.Empty;
}