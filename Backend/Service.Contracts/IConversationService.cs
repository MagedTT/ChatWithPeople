using Shared.DTOs;

namespace Service.Contracts;

public interface IConversationService
{
    Task<ConversationDto> GetConversationByIdAsync(Guid conversationId, bool trackChanges);
    Task<ConversationDto> GetOrCreateConversationBetweenUserIdAndFriendIdAsync(Guid userId, Guid friendId, bool trackChanges);
    // Task<ConversationDto> GetConversationByUserIdAndFriendIdAsync(Guid userId, Guid friendId, bool trackChanges);
    // Task<ConversationDto> CreateConversationBetweenUserIdAndFriendIdAsync(Guid userId, Guid friendId, bool trackChanges);
    Task DeleteConversationByIdAsync(Guid conversationId, bool trackChanges);
}