using Entities.Models;

namespace Contracts;

public interface IConversationRepository
{
    Task<Conversation?> GetConversationByIdAsync(Guid conversationId, bool trackChanges);
    Task<Conversation?> GetConversationByUserIdAndFriendIdAsync(Guid userId, Guid friendId, bool trackChanges);
    Task<bool> CheckIfConversationExistsByConversationIdAsync(Guid conversationId);
    void CreateConversation(Conversation conversation);
    void DeleteConversation(Conversation conversation);
}