namespace Entities.Exceptions;

public sealed class MessageNotFound : NotFoundException
{
    public MessageNotFound(string message)
        : base(message)
    { }
}