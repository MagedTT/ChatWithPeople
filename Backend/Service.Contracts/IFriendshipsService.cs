using Shared.DTOs;
using Shared.RequestFeatures;

namespace Service.Contracts;

public interface IFriendshipsService
{
    Task<FriendshipsDto?> GetFriendshipByUser1IdAndUser2IdAsync(Guid user1Id, Guid user2Id, bool trackChanges);
    Task<(IEnumerable<FriendshipsDto> friendshipDtos, MetaData metaData)> GetAllFriendsByUser1IdAsync(Guid user1Id, FriendshipsParameter friendshipParameter, bool trackChanges);

    Task CreateFriendshipBetweenUser1IdAndUser2Id(Guid user1Id, Guid user2Id);

    Task DeleteFriendshipByUser1Id(Guid user1Id, Guid user2Id, bool trackChanges);
}