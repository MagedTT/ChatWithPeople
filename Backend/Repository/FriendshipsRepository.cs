using System.Security.Cryptography.X509Certificates;
using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Utilities;
using Shared.RequestFeatures;

namespace Repository;

public class FriendshipsRepository : RepositoryBase<Friendship>, IFriendshipsRepository
{

    public FriendshipsRepository(ApplicationDbContext context)
        : base(context)
    { }

    public async Task<PagedList<Friendship>> GetAllFriendsByUser1IdAsync(Guid user1Id, FriendshipsParameter friendshipParameter, bool trackChanges)
    {
        List<Friendship> friendships = await FindByCondition(x => x.User1Id == user1Id && x.User2.IsDeleted == false, trackChanges: trackChanges)
            .Search(friendshipParameter.SearchTerm ?? "")
            .Skip((friendshipParameter.PageNumber - 1) * friendshipParameter.PageSize)
            .Take(friendshipParameter.PageSize)
            .Include(x => x.User2)
            .ToListAsync();

        int count = await _context.Friendships.CountAsync();

        return new PagedList<Friendship>(friendships, count, friendshipParameter.PageNumber, friendshipParameter.PageSize);
    }

    public async Task<Friendship?> GetFriendshipByUser1IdAndUser2IdAsync(Guid user1Id, Guid user2Id, bool trackChanges)
        => await FindByCondition(x => x.User1Id.Equals(user1Id) && x.User2Id.Equals(user2Id), trackChanges).FirstOrDefaultAsync();

    public void CreateFriendship(Friendship friendship)
        => Create(friendship);

    public void DeleteFriendship(Friendship friendship)
        => Delete(friendship);
}