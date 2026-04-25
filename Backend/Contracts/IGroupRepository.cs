using Entities.Models;
using Shared.RequestFeatures;

namespace Contracts;

public interface IGroupRepository
{
    Task<Group?> GetGroupByIdAsync(Guid groupId, bool trackChanges = false);
    Task<PagedList<Group>> GetAllPublicGroupsAsync(GroupParameters groupParameters, bool trackChanges);
    Task<PagedList<Group>> GetAllPublicAndMemberGroupsByUserIdAsync(Guid userId, GroupParameters groupParameters, bool trackChanges);
    void CreateGroup(Group group);
    void DeleteGroup(Group group);
}