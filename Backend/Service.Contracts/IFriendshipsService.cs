using Shared.DTOs;
using Shared.RequestFeatures;

namespace Service.Contracts;

public interface IFriendshipsService
{
    Task<FriendshipsDto?> GetFriendshipByUser1IdAndUser2IdAsync(Guid user1Id, Guid user2Id, bool trackChanges);
    Task<(IEnumerable<FriendshipsDto> friendshipDtos, MetaData metaData)> GetAllFriendsByUserIdAsync(Guid userId, FriendshipsParameters friendshipParameters, bool trackChanges);
    Task CreateFriendshipBetweenUser1IdAndUser2Id(Guid user1Id, Guid user2Id);
    Task<IEnumerable<UserMinimalInformationDto>> GetFriendsMinimalInformationByUserIdAsync(Guid userId, bool trackChanges);
    Task<IEnumerable<UserMinimalInformationDto>> GetFriendsWithMinimalInformationByUserIdAsync(Guid userId, bool trackChanges);
    Task DeleteFriendshipByUsersIds(Guid user1Id, Guid user2Id, bool trackChanges);
}