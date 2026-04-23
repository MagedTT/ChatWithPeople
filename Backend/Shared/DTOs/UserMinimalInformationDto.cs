namespace Shared.DTOs;

public class UserMinimalInformationDto
{
    public Guid Id { get; set; }
    public byte[]? ProfilePicture { get; set; }
    public string UserName { get; set; } = string.Empty;
}