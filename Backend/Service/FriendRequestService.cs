using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Service.Contracts;
using Shared.DTOs;
using Shared.RequestFeatures;

namespace Service;

public class FriendRequestService : IFriendRequestService
{
    private readonly UserManager<User> _userManager;
    private readonly IRepositoryManager _repositoryManager;
    private readonly ILoggerManager _loggerManager;
    private readonly IMapper _mapper;

    public FriendRequestService(UserManager<User> userManager, IRepositoryManager repositoryManager, ILoggerManager loggerManager, IMapper mapper)
    {
        _repositoryManager = repositoryManager;
        _loggerManager = loggerManager;
        _userManager = userManager;
        _mapper = mapper;
    }

    // public async Task<(IEnumerable<FriendRequestDto> friendRequestDtos, MetaData metaData)> GetFriendRequestsAsync(Guid userId, FriendRequestParameters friendRequestParameters, bool trackChanges)
    // {
    //     await CheckIfUserExists(userId);

    //     PagedList<FriendRequest> friendRequestsWithMetaData = await _repositoryManager.FriendRequestRepository.GetFriendRequestsAsync(userId, friendRequestParameters, trackChanges);

    //     IEnumerable<FriendRequestDto> friendRequestDtos = _mapper.Map<IEnumerable<FriendRequestDto>>(friendRequestsWithMetaData);

    //     return (friendRequestDtos, metaData: friendRequestsWithMetaData.MetaData);
    // }

    public async Task<(IEnumerable<Shared.DTOs.FriendRequestDto> friendRequestDtos, MetaData metaData)> GetFriendRequestsAsync(Guid userId, FriendRequestParameters friendRequestParameters, bool sent = false)
    {
        PagedList<Entities.Models.FriendRequestDto> friendRequestsDtosWithMetaData = await _repositoryManager.FriendRequestRepository.GetFriendRequestsByUserIdAsync(userId, friendRequestParameters, sent);

        IEnumerable<Shared.DTOs.FriendRequestDto> friendRequestDtos = _mapper.Map<IEnumerable<Shared.DTOs.FriendRequestDto>>(friendRequestsDtosWithMetaData);

        return (friendRequestDtos, metaData: friendRequestsDtosWithMetaData.MetaData);
    }

    public async Task AcceptFriendRequest(Guid friendRequestId)
    {
        FriendRequest friendRequest = await GetFriendRequestAndCheckIfItExistsAsync(friendRequestId);

        (Guid user1Id, Guid user2Id) = OrderUsersIds(friendRequest.SenderId, friendRequest.ReceiverId);

        Friendship friendship = new Friendship
        {
            User1Id = user1Id,
            User2Id = user2Id,
            CreatedAt = DateTime.Now
        };

        _repositoryManager.FriendshipsRepository.CreateFriendship(friendship);

        _repositoryManager.FriendRequestRepository.DeleteFriendRequest(friendRequest);
        await _repositoryManager.SaveAsync();
    }

    public async Task CreateFriendRequest(Guid senderId, Guid receiverId)
    {
        await CheckIfUserExists(senderId);
        await CheckIfUserExists(receiverId);

        if (await _repositoryManager.FriendshipsRepository.FriendshipExistsAsync(senderId, receiverId))
            throw new FriendRequestBadRequest("Already Friends");

        (string message, bool status) = await _repositoryManager.FriendRequestRepository.CheckIfValidFriendRequestAsync(senderId, receiverId);

        if (!status)
            throw new FriendRequestBadRequest(message);

        FriendRequest friendRequest = new FriendRequest
        {
            SenderId = senderId,
            ReceiverId = receiverId,
            FriendRequestStatus = Entities.Enums.FriendRequestStatus.Pending,
            CreatedAt = DateTime.Now
        };

        _repositoryManager.FriendRequestRepository.CreateFriendRequest(friendRequest);
        await _repositoryManager.SaveAsync();
    }

    public async Task DeleteFriendRequest(Guid friendRequestId)
    {
        FriendRequest friendRequest = await GetFriendRequestAndCheckIfItExistsAsync(friendRequestId);

        _repositoryManager.FriendRequestRepository.DeleteFriendRequest(friendRequest);

        await _repositoryManager.SaveAsync();
    }

    private async Task CheckIfUserExists(Guid userId)
    {
        if (!await _userManager.Users.AnyAsync(x => x.Id == userId))
            throw new UserNotFoundException(userId);
    }

    private async Task<FriendRequest> GetFriendRequestAndCheckIfItExistsAsync(Guid friendRequestId)
    {
        FriendRequest? friendRequest = await _repositoryManager.FriendRequestRepository.GetFriendRequestByIdAsync(friendRequestId);

        if (friendRequest is null)
            throw new FriendRequestNotFound(friendRequestId);

        return friendRequest;
    }

    private (Guid smallerUserId, Guid biggerUserId) OrderUsersIds(Guid user1Id, Guid user2Id)
        => user1Id > user2Id ? (user2Id, user1Id) : (user1Id, user2Id);

    public async Task<int> GetFriendRequestsTotalCountAsync(Guid userId)
        => await _repositoryManager.FriendRequestRepository.GetFriendRequestsTotalCountAsync(userId);

    public async Task<int> GetFriendRequestsSentCountAsync(Guid userId)
        => await _repositoryManager.FriendRequestRepository.GetFriendRequestsSentCountAsync(userId);

    public async Task<int> GetFriendRequestsReceivedCountAsync(Guid userId)
        => await _repositoryManager.FriendRequestRepository.GetFriendRequestsReceivedCountAsync(userId);
}