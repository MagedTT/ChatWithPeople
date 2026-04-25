using Entities.Models;

namespace Contracts;

public interface IGroupMemberRepository
{
    void CreateGroupMemberForGroup(Guid groupId, GroupMember groupMember);
}