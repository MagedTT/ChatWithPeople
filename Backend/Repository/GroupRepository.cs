using System.Diagnostics.CodeAnalysis;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Utilities;
using Shared.RequestFeatures;

namespace Repository;

public class GroupRepository : RepositoryBase<Group>, IGroupRepository
{
    public GroupRepository(ApplicationDbContext context)
        : base(context)
    { }

    public async Task<Group?> GetGroupByIdAsync(Guid groupId, bool trackChanges = false)
        => await FindByCondition(x => x.Id.Equals(groupId), trackChanges).FirstOrDefaultAsync();

    public async Task<PagedList<Group>> GetAllPublicGroupsAsync(GroupParameters groupParameters, bool trackChanges)
    {
        List<Group> groups =
            await FindByCondition(x => x.IsPublic == true)
            .Search(groupParameters.GroupNameForSearching ?? "")
            .Skip((groupParameters.PageNumber - 1) * groupParameters.PageSize)
            .Take(groupParameters.PageSize)
            .ToListAsync();

        int count = await FindByCondition(x => x.IsPublic == true).CountAsync();

        return new PagedList<Group>(groups, count, groupParameters.PageNumber, groupParameters.PageSize);
    }

    public async Task<PagedList<Group>> GetAllPublicAndMemberGroupsByUserIdAsync(Guid userId, GroupParameters groupParameters, bool trackChanges)
    {
        List<Group> groups =
            await FindByCondition(x => x.IsPublic == true || x.Members.Any(m => m.UserId.Equals(userId)))
            .Search(groupParameters.GroupNameForSearching ?? "")
            .Skip((groupParameters.PageNumber - 1) * groupParameters.PageSize)
            .Take(groupParameters.PageSize)
            .ToListAsync();

        int count = await FindByCondition(x => x.IsPublic == true || x.Members.Any(m => m.UserId.Equals(userId))).CountAsync();

        return new PagedList<Group>(groups, count, groupParameters.PageNumber, groupParameters.PageSize);
    }

    public void CreateGroup(Group group)
        => Create(group);

    public void DeleteGroup(Group group)
        => Delete(group);
}