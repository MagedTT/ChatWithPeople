using Shared.DTOs;
using Shared.RequestFeatures;

namespace Service.Contracts;

public interface IGroupService
{
    Task<(IEnumerable<GroupDto> groupsDtos, MetaData metaData)> GetAllPublicGroupsAsync(GroupParameters groupParameters, bool trackChanges);
    Task<(IEnumerable<GroupDto> groupsDtos, MetaData metaData)> GetAllPublicAndMemberGroupsByUserIdAsync(Guid userId, GroupParameters groupParameters, bool trackChanges);
    Task CreateGroupForUserWithIdAsync(Guid userId, GroupForCreationDto groupForCreationDto);
    Task DeleteGroupForUserWithIdAsync(Guid userId, Guid groupId, bool trackChanges);
}