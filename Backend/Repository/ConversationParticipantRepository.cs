using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository;

public class ConversationParticipantRepository : RepositoryBase<ConversationParticipant>, IConversationParticipantRepository
{
    public ConversationParticipantRepository(ApplicationDbContext context)
        : base(context)
    { }

    public async Task<IEnumerable<ConversationParticipant>> GetAllConversationParticipantsExceptSenderByConversationIdAsync(Guid conversationId, Guid senderId, bool trackChanges)
        => await FindByCondition(x => x.ConversationId.Equals(conversationId) && !x.UserId.Equals(senderId), trackChanges).ToListAsync();

    public void AddConversationParticipantsAsync(IEnumerable<ConversationParticipant> conversationParticipants)
    {
        foreach (ConversationParticipant conversationParticipant in conversationParticipants)
            Create(conversationParticipant);
    }
}