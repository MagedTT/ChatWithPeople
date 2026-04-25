using Contracts;
using Entities.Models;

namespace Repository;

public class GroupMemberRepository : RepositoryBase<GroupMember>, IGroupMemberRepository
{
    public GroupMemberRepository(ApplicationDbContext context)
        : base(context)
    { }

    public void CreateGroupMemberForGroup(Guid groupId, GroupMember groupMember)
    {
        groupMember.GroupId = groupId;

        Create(groupMember);
    }
}