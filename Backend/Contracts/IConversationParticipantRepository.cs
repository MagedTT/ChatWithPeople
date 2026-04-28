using Entities.Models;

namespace Contracts;

public interface IConversationParticipantRepository
{
    Task<IEnumerable<ConversationParticipant>> GetAllConversationParticipantsExceptSenderByConversationIdAsync(Guid conversationId, Guid SenderId, bool trackChanges);
    void AddConversationParticipantsAsync(IEnumerable<ConversationParticipant> conversationParticipants);
}