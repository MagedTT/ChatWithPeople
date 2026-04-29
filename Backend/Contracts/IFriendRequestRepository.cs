using Entities.Models;
using Shared.RequestFeatures;

namespace Contracts;

public interface IFriendRequestRepository
{
    Task<FriendRequest?> GetFriendRequestByIdAsync(Guid friendRequestId);
    Task<int> GetFriendRequestsTotalCountAsync(Guid userId);
    Task<int> GetFriendRequestsSentCountAsync(Guid userId);
    Task<int> GetFriendRequestsReceivedCountAsync(Guid userId);
    Task<PagedList<FriendRequestDto>> GetFriendRequestsByUserIdAsync(Guid userId, FriendRequestParameters friendRequestParameters, bool sent = false);
    Task<(string message, bool status)> CheckIfValidFriendRequestAsync(Guid senderId, Guid receiverId);
    // Task<PagedList<FriendRequest>> GetFriendRequestsAsync(Guid userId, FriendRequestParameters friendRequestParameters, bool trackChanges);
    void CreateFriendRequest(FriendRequest friendRequest);
    void DeleteFriendRequest(FriendRequest friendRequest);
}