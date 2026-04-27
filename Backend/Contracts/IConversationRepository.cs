using Entities.Models;

namespace Contracts;

public interface IConversationRepository
{
    Task<Conversation?> GetConversationByIdAsync(Guid conversationId, bool trackChanges);
    Task<Conversation?> GetConversationByUserIdAndFriendIdAsync(Guid userId, Guid friendId, bool trackChanges);
    void CreateConversation(Conversation conversation);
    void DeleteConversation(Conversation conversation);
}