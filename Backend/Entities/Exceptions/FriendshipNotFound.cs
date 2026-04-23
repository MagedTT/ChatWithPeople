namespace Entities.Exceptions;

public sealed class FriendshipNotFound : NotFoundException
{
    public FriendshipNotFound(Guid user1Id, Guid user2Id)
        : base($"No Friendship between {user1Id} and {user2Id}")
    { }
}