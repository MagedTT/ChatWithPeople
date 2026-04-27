using Contracts;
using Entities.Models;

namespace Repository;

public class ConversationParticipantRepository : RepositoryBase<ConversationParticipant>, IConversationParticipantRepository
{
    public ConversationParticipantRepository(ApplicationDbContext context)
        : base(context)
    { }

    public void AddConversationParticipantsAsync(IEnumerable<ConversationParticipant> conversationParticipants)
    {
        foreach (ConversationParticipant conversationParticipant in conversationParticipants)
            Create(conversationParticipant);
    }
}