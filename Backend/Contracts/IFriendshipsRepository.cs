using Entities.Models;
using Shared.DTOs;
using Shared.RequestFeatures;

namespace Contracts;

public interface IFriendshipsRepository
{
    Task<Friendship?> GetFriendshipByUser1IdAndUser2IdAsync(Guid user1Id, Guid user2Id, bool trackChanges);
    Task<bool> FriendshipExistsAsync(Guid user1Id, Guid user2Id);
    Task<IEnumerable<Friendship>> GetFriendUsersMinimalInformation(Guid userId, bool trackChanges);
    Task<IEnumerable<User>> GetFriendsMinimalInformationByUserIdAsync(Guid userId, bool trackChanges);
    Task<int> GetTotalFriendsCountByUserIdAsync(Guid userId);
    Task<PagedList<Friend>> GetAllFriendsByUserIdAsync(Guid userId, FriendshipsParameters friendshipParameters, bool trackChanges);
    Task<IEnumerable<FriendWithLastSentMessage>> GetFriendsWithLastMessageByUserIdAsync(Guid userId, string? searchTerm);
    void CreateFriendship(Friendship friendship);
    void DeleteFriendship(Friendship friendship);
}