using System.Security.Cryptography.X509Certificates;
using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Service.Contracts;
using Shared.DTOs;
using Shared.RequestFeatures;

namespace Service;

public class FriendshipsService : IFriendshipsService
{
    private readonly UserManager<User> _userManager;
    private readonly IMapper _mapper;
    private readonly IRepositoryManager _repositoryManager;
    private readonly ILoggerManager _loggerManager;

    public FriendshipsService(
        UserManager<User> userManager,
        IMapper mapper,
        IRepositoryManager repositoryManager,
        ILoggerManager loggerManager)
    {
        _userManager = userManager;
        _mapper = mapper;
        _repositoryManager = repositoryManager;
        _loggerManager = loggerManager;
    }

    public async Task<(IEnumerable<FriendDto> friendDtos, MetaData metaData)> GetAllFriendsByUserIdAsync(Guid userId, FriendshipsParameters friendshipsParameters, bool trackChanges)
    {
        await CheckIfUserExistsAsync(userId);

        PagedList<Friend> friendsWithMetaData = await _repositoryManager.FriendshipsRepository.GetAllFriendsByUserIdAsync(userId, friendshipsParameters, trackChanges);

        IEnumerable<FriendDto> friendDtos = _mapper.Map<IEnumerable<FriendDto>>(friendsWithMetaData);

        return (friendDtos, metaData: friendsWithMetaData.MetaData);
    }

    public async Task<FriendshipsDto?> GetFriendshipByUser1IdAndUser2IdAsync(Guid user1Id, Guid user2Id, bool trackChanges)
    {
        Friendship friendship = await GetFriendshipByUser1IAnsUser2IdAndCheckIfItExists(user1Id, user2Id, trackChanges);

        return _mapper.Map<FriendshipsDto>(friendship);
    }

    public async Task CreateFriendshipBetweenUser1IdAndUser2Id(Guid user1Id, Guid user2Id)
    {
        await CheckIfUserExistsAsync(user1Id);
        await CheckIfUserExistsAsync(user2Id);

        (user1Id, user2Id) = OrderUsersIds(user1Id, user2Id);

        Friendship friendship = new Friendship
        {
            User1Id = user1Id,
            User2Id = user2Id,
            CreatedAt = DateTime.Now
        };

        _repositoryManager.FriendshipsRepository.CreateFriendship(friendship);

        await _repositoryManager.SaveAsync();
    }

    public async Task DeleteFriendshipByUsersIds(Guid user1Id, Guid user2Id, bool trackChanges)
    {
        await CheckIfUserExistsAsync(user1Id);
        await CheckIfUserExistsAsync(user2Id);

        Friendship friendship = await GetFriendshipByUser1IAnsUser2IdAndCheckIfItExists(user1Id, user2Id, trackChanges);

        _repositoryManager.FriendshipsRepository.DeleteFriendship(friendship);

        Conversation? conversation = await _repositoryManager.ConversationRepository.GetConversationByUserIdAndFriendIdAsync(user1Id, user2Id, trackChanges: false);

        if (conversation is not null)
            _repositoryManager.ConversationRepository.DeleteConversation(conversation);

        await _repositoryManager.SaveAsync();
    }

    private async Task CheckIfUserExistsAsync(Guid userId)
    {
        if (await _userManager.Users.FirstOrDefaultAsync(x => x.Id.Equals(userId)) is null)
            throw new UserNotFoundException(userId);
    }

    private async Task<Friendship> GetFriendshipByUser1IAnsUser2IdAndCheckIfItExists(Guid user1Id, Guid user2Id, bool trackChanges)
    {
        (user1Id, user2Id) = OrderUsersIds(user1Id, user2Id);

        Friendship? friendship = await _repositoryManager.FriendshipsRepository.GetFriendshipByUser1IdAndUser2IdAsync(user1Id, user2Id, trackChanges);

        if (friendship is null)
            throw new FriendshipNotFound(user1Id, user2Id);

        return friendship;
    }

    public async Task<IEnumerable<FriendWithLastSentMessageDto>> GetFriendsWithLastMessageByUserIdAsync(Guid userId, string? searchTerm)
    {
        IEnumerable<FriendWithLastSentMessage> friendWithLastSentMessages = await _repositoryManager.FriendshipsRepository.GetFriendsWithLastMessageByUserIdAsync(userId, searchTerm);

        IEnumerable<FriendWithLastSentMessageDto> friendWithLastSentMessagesDto = _mapper.Map<IEnumerable<FriendWithLastSentMessageDto>>(friendWithLastSentMessages);

        return friendWithLastSentMessagesDto;
    }

    public async Task<IEnumerable<UserMinimalInformationDto>> GetFriendsWithMinimalInformationByUserIdAsync(Guid userId, bool trackChanges)
    {
        IEnumerable<Friendship> friendships = await _repositoryManager.FriendshipsRepository.GetFriendUsersMinimalInformation(userId, trackChanges);

        IEnumerable<UserMinimalInformationDto> usersWithMinimalInformation = _mapper.Map<IEnumerable<UserMinimalInformationDto>>(friendships.SelectMany(x => new[] { x.User1, x.User2 })).DistinctBy(x => x.Id).Where(x => x.Id != userId);

        return usersWithMinimalInformation;
    }

    public async Task<IEnumerable<UserMinimalInformationDto>> GetFriendsMinimalInformationByUserIdAsync(Guid userId, bool trackChanges)
    {
        IEnumerable<User> friends = await _repositoryManager.FriendshipsRepository.GetFriendsMinimalInformationByUserIdAsync(userId, trackChanges);

        IEnumerable<UserMinimalInformationDto> userMinimalInformationDtos = _mapper.Map<IEnumerable<UserMinimalInformationDto>>(friends);

        return userMinimalInformationDtos;
    }

    private (Guid smallerUserId, Guid biggerUserId) OrderUsersIds(Guid user1Id, Guid user2Id)
        => user1Id > user2Id ? (user2Id, user1Id) : (user1Id, user2Id);

    public async Task<int> GetTotalFriendsCountByUserIdAsync(Guid userId)
    {
        return await _repositoryManager.FriendshipsRepository.GetTotalFriendsCountByUserIdAsync(userId);
    }
}