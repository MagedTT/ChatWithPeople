using Entities.Models;

namespace Repository.Utilities;

public static class FriendshipRepositoryExtensions
{
    public static IQueryable<Friendship> Search(this IQueryable<Friendship> friendships, string searchTerm)
    {
        if (string.IsNullOrEmpty(searchTerm))
            return friendships;

        string lowerCaseSearchTerm = searchTerm.ToLower().Trim();

        return friendships.Where(x => x.User2.UserName != null && x.User2.UserName.Equals(searchTerm));
    }
}