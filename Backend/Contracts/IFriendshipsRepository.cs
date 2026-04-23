using Entities.Models;
using Shared.RequestFeatures;

namespace Contracts;

public interface IFriendshipsRepository
{
    Task<Friendship?> GetFriendshipByUser1IdAndUser2IdAsync(Guid user1Id, Guid user2Id, bool trackChanges);
    Task<PagedList<Friendship>> GetAllFriendsByUser1IdAsync(Guid user1Id, FriendshipsParameter friendshipParameter, bool trackChanges);
    void CreateFriendship(Friendship friendship);
    void DeleteFriendship(Friendship friendship);
}