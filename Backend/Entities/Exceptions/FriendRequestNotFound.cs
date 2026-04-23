namespace Entities.Exceptions;

public sealed class FriendRequestNotFound : NotFoundException
{

    public FriendRequestNotFound(Guid friendRequestId)
        : base($"Friend Request with Id: {friendRequestId} not found")
    {
    }
}