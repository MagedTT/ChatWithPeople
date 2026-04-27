namespace Entities.Exceptions;

public class ConversationNotFound : NotFoundException
{
    public ConversationNotFound(string message)
        : base(message)
    { }
}