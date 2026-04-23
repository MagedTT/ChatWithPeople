using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Shared.RequestFeatures;

namespace Repository;

public class FriendRequestRepository : RepositoryBase<FriendRequest>, IFriendRequestRepository
{

    public FriendRequestRepository(ApplicationDbContext context)
        : base(context)
    { }

    public async Task<FriendRequest?> GetFriendRequestByIdAsync(Guid friendRequestId)
        => await FindByCondition(x => x.Id.Equals(friendRequestId)).FirstOrDefaultAsync();

    public async Task<PagedList<FriendRequest>> GetFriendRequestsAsync(Guid userId, FriendRequestParameters friendRequestParameters, bool trackChanges)
    {
        List<FriendRequest> friendRequests =
            await FindByCondition(x => friendRequestParameters.Sent ? x.SenderId.Equals(userId) : x.ReceiverId.Equals(userId), trackChanges)
            .Skip((friendRequestParameters.PageNumber - 1) * friendRequestParameters.PageSize)
            .Take(friendRequestParameters.PageSize)
            .Include(x => friendRequestParameters.Sent ? x.Receiver : x.Sender)
            .ToListAsync();

        int count =
            await FindByCondition(x => friendRequestParameters.Sent
                ? x.SenderId.Equals(userId)
                : x.ReceiverId.Equals(userId),
                trackChanges).CountAsync();

        return new PagedList<FriendRequest>(friendRequests, count, friendRequestParameters.PageNumber, friendRequestParameters.PageSize);
    }

    public void CreateFriendRequest(FriendRequest friendRequest)
        => Create(friendRequest);

    public void DeleteFriendRequest(FriendRequest friendRequest)
        => Delete(friendRequest);

    public async Task<(string message, bool status)> CheckIfValidFriendRequestAsync(Guid senderId, Guid receiverId)
    {
        if (senderId.Equals(receiverId))
            return ("An account can not send a friend requeset to itself", false);

        if (
            await FindByCondition(x =>
                (x.SenderId.Equals(senderId) && x.ReceiverId.Equals(receiverId)) ||
                (x.SenderId.Equals(receiverId) && x.ReceiverId.Equals(senderId)), trackChanges: false)
                .AnyAsync()
            )
            return ("friend requiest already exists", false);

        return ("", true);
    }

    public async Task<int> GetFriendRequestsTotalCountAsync(Guid userId)
        => await FindByCondition(x => x.SenderId.Equals(userId) || x.ReceiverId.Equals(userId)).CountAsync();

    public async Task<int> GetFriendRequestsSentCountAsync(Guid userId)
        => await FindByCondition(x => x.SenderId.Equals(userId)).CountAsync();

    public async Task<int> GetFriendRequestsReceivedCountAsync(Guid userId)
        => await FindByCondition(x => x.ReceiverId.Equals(userId)).CountAsync();
}