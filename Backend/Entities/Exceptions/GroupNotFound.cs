namespace Entities.Exceptions;

public sealed class GroupNotFound : NotFoundException
{
    public GroupNotFound(Guid groupId)
        : base($"Group with Id: {groupId} not found")
    { }
}