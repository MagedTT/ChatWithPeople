using Shared.DTOs;
using Shared.RequestFeatures;

namespace Service.Contracts;

public interface IFriendRequestService
{
    Task<(IEnumerable<FriendRequestDto> friendRequestDtos, MetaData metaData)> GetFriendRequestsAsync(Guid userId, FriendRequestParameters friendRequestParameters, bool trackChanges);
    Task<int> GetFriendRequestsTotalCountAsync(Guid userId);
    Task<int> GetFriendRequestsSentCountAsync(Guid userId);
    Task<int> GetFriendRequestsReceivedCountAsync(Guid userId);
    Task CreateFriendRequest(Guid senderId, Guid receiverId);
    Task AcceptFriendRequest(Guid friendRequestId);
    Task DeleteFriendRequest(Guid friendRequestId);
}