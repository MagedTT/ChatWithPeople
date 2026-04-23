namespace Entities.Exceptions;

public sealed class FriendRequestBadRequest : BadRequestException
{
    public FriendRequestBadRequest(string message)
        : base(message)
    { }
}