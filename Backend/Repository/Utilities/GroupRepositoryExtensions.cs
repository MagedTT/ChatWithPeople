using Entities.Models;

namespace Repository.Utilities;

public static class GroupRepositoryExtensions
{
    public static IQueryable<Group> Search(this IQueryable<Group> groups, string searchTerm)
    {
        if (string.IsNullOrEmpty(searchTerm))
            return groups;

        string lowerCaseSearchTerm = searchTerm.ToLower().Trim();

        return groups.Where(x => x.Name.ToLower().Contains(lowerCaseSearchTerm));
    }
}