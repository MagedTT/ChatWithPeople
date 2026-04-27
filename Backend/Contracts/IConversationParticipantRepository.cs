using Entities.Models;

namespace Contracts;

public interface IConversationParticipantRepository
{
    void AddConversationParticipantsAsync(IEnumerable<ConversationParticipant> conversationParticipants);
}