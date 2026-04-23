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

    public async Task<(IEnumerable<FriendshipsDto> friendshipDtos, MetaData metaData)> GetAllFriendsByUser1IdAsync(Guid user1Id, FriendshipsParameter friendshipsParameter, bool trackChanges)
    {
        await CheckIfUserExistsAsync(user1Id);

        PagedList<Friendship> friendshipsWithMetaData = await _repositoryManager.FriendshipsRepository.GetAllFriendsByUser1IdAsync(user1Id, friendshipsParameter, trackChanges);

        IEnumerable<FriendshipsDto> friendshipsDtos = _mapper.Map<IEnumerable<FriendshipsDto>>(friendshipsWithMetaData);

        return (friendshipDtos: friendshipsDtos, metaData: friendshipsWithMetaData.MetaData);
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

        Friendship friendship = new Friendship
        {
            User1Id = user1Id,
            User2Id = user2Id,
            CreatedAt = DateTime.Now
        };

        _repositoryManager.FriendshipsRepository.CreateFriendship(friendship);

        await _repositoryManager.SaveAsync();
    }

    public async Task DeleteFriendshipByUser1Id(Guid user1Id, Guid user2Id, bool trackChanges)
    {
        await CheckIfUserExistsAsync(user1Id);
        await CheckIfUserExistsAsync(user2Id);

        Friendship friendship = await GetFriendshipByUser1IAnsUser2IdAndCheckIfItExists(user1Id, user2Id, trackChanges);

        _repositoryManager.FriendshipsRepository.DeleteFriendship(friendship);
        await _repositoryManager.SaveAsync();
    }

    private async Task CheckIfUserExistsAsync(Guid userId)
    {
        if (await _userManager.Users.FirstOrDefaultAsync(x => x.Id.Equals(userId)) is null)
            throw new UserNotFoundException(userId);
    }

    private async Task<Friendship> GetFriendshipByUser1IAnsUser2IdAndCheckIfItExists(Guid user1Id, Guid user2Id, bool trackChanges)
    {
        Friendship? friendship = await _repositoryManager.FriendshipsRepository.GetFriendshipByUser1IdAndUser2IdAsync(user1Id, user2Id, trackChanges);

        if (friendship is null)
            throw new FriendshipNotFound(user1Id, user2Id);

        return friendship;
    }
}