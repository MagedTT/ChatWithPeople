using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs;
using Shared.RequestFeatures;

namespace Repository;

public class FriendRequestRepository : RepositoryBase<FriendRequest>, IFriendRequestRepository
{

    public FriendRequestRepository(ApplicationDbContext context)
        : base(context)
    { }

    public async Task<FriendRequest?> GetFriendRequestByIdAsync(Guid friendRequestId)
        => await FindByCondition(x => x.Id.Equals(friendRequestId)).FirstOrDefaultAsync();

    public async Task<PagedList<Entities.Models.FriendRequestDto>> GetFriendRequestsByUserIdAsync(Guid userId, FriendRequestParameters friendRequestParameters, bool sent = false)
    {
        List<Entities.Models.FriendRequestDto> friendRequestDtos = await _context.FriendRequests.Where(x => sent ? x.SenderId.Equals(userId) : x.ReceiverId.Equals(userId))
            .Skip((friendRequestParameters.PageNumber - 1) * friendRequestParameters.PageSize)
            .Take(friendRequestParameters.PageSize)
            .Select(x => sent ? new Entities.Models.FriendRequestDto { Id = x.Id, UserId = x.ReceiverId, UserName = x.Receiver.UserName!, ProfilePicture = x.Receiver.ProfilePicture != null ? Convert.ToBase64String(x.Receiver.ProfilePicture) : null, FriendRequestStatus = x.FriendRequestStatus, CreatedAt = x.CreatedAt } : new Entities.Models.FriendRequestDto { Id = x.Id, UserId = x.SenderId, UserName = x.Sender.UserName!, ProfilePicture = x.Sender.ProfilePicture != null ? Convert.ToBase64String(x.Sender.ProfilePicture) : null, FriendRequestStatus = x.FriendRequestStatus, CreatedAt = x.CreatedAt }).ToListAsync();

        int count = await _context.FriendRequests.Where(x => sent ? x.SenderId.Equals(userId) : x.ReceiverId.Equals(userId)).CountAsync();

        return new PagedList<Entities.Models.FriendRequestDto>(friendRequestDtos, count, friendRequestParameters.PageNumber, friendRequestParameters.PageSize);
    }

    // public async Task<PagedList<FriendRequest>> GetFriendRequestsAsync(Guid userId, FriendRequestParameters friendRequestParameters, bool trackChanges)
    // {
    //     List<FriendRequest> friendRequests =
    //         await FindByCondition(x => friendRequestParameters.Sent ? x.SenderId.Equals(userId) : x.ReceiverId.Equals(userId), trackChanges)
    //         .Skip((friendRequestParameters.PageNumber - 1) * friendRequestParameters.PageSize)
    //         .Take(friendRequestParameters.PageSize)
    //         .Include(x => friendRequestParameters.Sent ? x.Receiver : x.Sender)
    //         .ToListAsync();

    //     int count =
    //         await FindByCondition(x => friendRequestParameters.Sent
    //             ? x.SenderId.Equals(userId)
    //             : x.ReceiverId.Equals(userId),
    //             trackChanges).CountAsync();

    //     return new PagedList<FriendRequest>(friendRequests, count, friendRequestParameters.PageNumber, friendRequestParameters.PageSize);
    // }

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

    private int GetMutualFriends(Guid user1Id, Guid user2Id)
    {
        IEnumerable<Guid> user1FriendsIds = _context.Friendships.Where(x => x.User1Id.Equals(user1Id) && x.User2.IsDeleted == false || x.User2Id.Equals(user1Id) && x.User1.IsDeleted == false).Select(x => x.User1Id.Equals(user1Id) ? x.User2Id : x.User1Id).ToList();

        IEnumerable<Guid> user2FriendsIds = _context.Friendships.Where(x => x.User1Id.Equals(user2Id) && x.User2.IsDeleted == false || x.User2Id.Equals(user2Id) && x.User1.IsDeleted == false).Select(x => x.User1Id.Equals(user2Id) ? x.User2Id : x.User1Id).ToList();

        return user1FriendsIds.Intersect(user2FriendsIds).ToList().Count();
    }
}