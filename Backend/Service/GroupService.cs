using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Service.Contracts;
using Shared.DTOs;
using Shared.RequestFeatures;

namespace Service;

public class GroupService : IGroupService
{
    private readonly UserManager<User> _userManager;
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _mapper;
    private readonly ILoggerManager _loggerManager;

    public GroupService(
        UserManager<User> userManager,
        IRepositoryManager repositoryManager,
        IMapper mapper,
        ILoggerManager loggerManager)
    {
        _userManager = userManager;
        _repositoryManager = repositoryManager;
        _mapper = mapper;
        _loggerManager = loggerManager;
    }

    public async Task<(IEnumerable<GroupDto> groupsDtos, MetaData metaData)> GetAllPublicGroupsAsync(GroupParameters groupParameters, bool trackChanges)
    {
        PagedList<Group> groupsWithMetaData = await _repositoryManager.GroupRepository.GetAllPublicGroupsAsync(groupParameters, trackChanges);

        IEnumerable<GroupDto> groupsDtos = _mapper.Map<IEnumerable<GroupDto>>(groupsWithMetaData);

        return (groupsDtos, metaData: groupsWithMetaData.MetaData);
    }

    public async Task<(IEnumerable<GroupDto> groupsDtos, MetaData metaData)> GetAllPublicAndMemberGroupsByUserIdAsync(Guid userId, GroupParameters groupParameters, bool trackChanges)
    {
        PagedList<Group> groupsWithMetaData = await _repositoryManager.GroupRepository.GetAllPublicAndMemberGroupsByUserIdAsync(userId, groupParameters, trackChanges);

        IEnumerable<GroupDto> groupsDtos = _mapper.Map<IEnumerable<GroupDto>>(groupsWithMetaData);

        return (groupsDtos, metaData: groupsWithMetaData.MetaData);
    }

    public async Task CreateGroupForUserWithIdAsync(Guid userId, GroupForCreationDto groupForCreationDto)
    {
        await CheckIfUserExists(userId);

        Group group = _mapper.Map<Group>(groupForCreationDto);

        if (groupForCreationDto.Avatar is not null)
            group.Avatar = await ConvertIFormFileToByteArray(groupForCreationDto.Avatar);

        _repositoryManager.GroupRepository.CreateGroup(group);

        ///////////////////////////////////////////////////////////////

        GroupMember groupMember = new GroupMember
        {
            Group = group,
            UserId = userId,
            MemberRole = Entities.Enums.GroupMemberRole.Admin,
            JoinedAt = DateTime.Now
        };

        _repositoryManager.GroupMemberRepository.CreateGroupMemberForGroup(group.Id, groupMember);

        ///////////////////////////////////////////////////////////////

        await _repositoryManager.SaveAsync();
    }

    public async Task DeleteGroupForUserWithIdAsync(Guid userId, Guid groupId, bool trackChanges)
    {
        await CheckIfUserExists(userId);

        Group group = await GetGroupAndCheckIfItExistsAsync(groupId, trackChanges);

        _repositoryManager.GroupRepository.DeleteGroup(group);

        await _repositoryManager.SaveAsync();
    }

    private async Task<Group> GetGroupAndCheckIfItExistsAsync(Guid groupId, bool trackChanges)
    {
        Group? group = await _repositoryManager.GroupRepository.GetGroupByIdAsync(groupId, trackChanges);

        if (group is null)
            throw new GroupNotFound(groupId);

        return group;
    }

    private async Task<byte[]> ConvertIFormFileToByteArray(IFormFile file)
    {
        using (MemoryStream memoryStream = new MemoryStream())
        {
            await file.CopyToAsync(memoryStream);

            return memoryStream.ToArray();
        }
    }

    private async Task CheckIfUserExists(Guid userId)
    {
        if (!await _userManager.Users.AnyAsync(x => x.Id.Equals(userId)))
            throw new UserNotFoundException(userId);
    }
}