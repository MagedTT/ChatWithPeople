namespace Entities.Exceptions;

public sealed class ConversationNotFound : NotFoundException
{
    public ConversationNotFound(string message)
        : base(message)
    { }
}