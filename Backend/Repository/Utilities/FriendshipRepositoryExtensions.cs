using Entities.Models;

namespace Repository.Utilities;

public static class FriendshipRepositoryExtensions
{
    public static IQueryable<Friendship> Search(this IQueryable<Friendship> friendships, bool user1, string searchTerm)
    {
        if (string.IsNullOrEmpty(searchTerm))
            return friendships;

        string lowerCaseSearchTerm = searchTerm.ToLower().Trim();

        return friendships.Where(x => x.User2.UserName != null && x.User2.UserName.ToLower().Contains(lowerCaseSearchTerm));
    }
}