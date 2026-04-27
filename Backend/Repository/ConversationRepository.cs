using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository;

public class ConversationRepository : RepositoryBase<Conversation>, IConversationRepository
{
    public ConversationRepository(ApplicationDbContext context)
        : base(context)
    { }

    public async Task<Conversation?> GetConversationByIdAsync(Guid conversationId, bool trackChanges)
        => await FindByCondition(x => x.Id.Equals(conversationId), trackChanges).FirstOrDefaultAsync();

    public async Task<Conversation?> GetConversationByUserIdAndFriendIdAsync(Guid userId, Guid friendId, bool trackChanges)
        => await FindByCondition(
            x => x.Type == Entities.Enums.ConversationType.Private &&
                x.ConversationParticipants.Any(cp => cp.UserId.Equals(userId) &&
                x.ConversationParticipants.Any(cp => cp.UserId.Equals(friendId)))
            ).FirstOrDefaultAsync();

    public void CreateConversation(Conversation conversation)
        => Create(conversation);

    public void DeleteConversation(Conversation conversation)
        => Delete(conversation);
}