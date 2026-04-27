using Entities.Models;
using Shared.RequestFeatures;

namespace Contracts;

public interface IMessageRepository
{
    Task<CursoredList<Message>> GetAllMessagesByConversationIdAsync(Guid conversationId, MessageParameters messageParameters, bool trackChanges);

    Task<Message?> GetMessageByidAsync(Guid messageId, bool trackChanges);
    void CreateMessage(Message message);
    void DeleteMessage(Message message);
}