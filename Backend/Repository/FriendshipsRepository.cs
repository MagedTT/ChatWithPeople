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

    public async Task<PagedList<Friend>> GetAllFriendsByUserIdAsync(Guid userId, FriendshipsParameters friendshipParameters, bool trackChanges)
    {
        List<Friend> friends = await FindByCondition(x => (x.User1Id.Equals(userId) && x.User2.IsDeleted == false && x.User2.UserName!.ToLower().Contains(friendshipParameters.SearchTerm ?? "")) || (x.User2Id.Equals(userId) && x.User1.IsDeleted == false && x.User1.UserName!.ToLower().Contains(friendshipParameters.SearchTerm ?? "")), trackChanges: trackChanges)
            .Skip((friendshipParameters.PageNumber - 1) * friendshipParameters.PageSize)
            .Take(friendshipParameters.PageSize)
            .Select(x => x.User1Id.Equals(userId) ? new Friend { FriendId = x.User2Id, UserName = x.User2.UserName!, ProfilePicture = x.User2.ProfilePicture != null ? Convert.ToBase64String(x.User2.ProfilePicture) : null, UserStatus = x.User2.Status } : new Friend { FriendId = x.User1Id, UserName = x.User1.UserName!, ProfilePicture = x.User1.ProfilePicture != null ? Convert.ToBase64String(x.User1.ProfilePicture) : null, UserStatus = x.User1.Status })
            .ToListAsync();

        int count =
             await FindByCondition(x => (x.User1Id.Equals(userId) && x.User2.IsDeleted == false && x.User2.UserName!.ToLower().Contains(friendshipParameters.SearchTerm ?? "")) || (x.User2Id.Equals(userId) && x.User1.IsDeleted == false && x.User1.UserName!.ToLower().Contains(friendshipParameters.SearchTerm ?? "")), trackChanges: trackChanges)
            .CountAsync();

        return new PagedList<Friend>(friends, count, friendshipParameters.PageNumber, friendshipParameters.PageSize);
    }

    public async Task<IEnumerable<User>> GetFriendsMinimalInformationByUserIdAsync(Guid userId, bool trackChanges)
    {
        IEnumerable<User> friends = await _context.Friendships
            .Where(x =>
                x.User1Id.Equals(userId) && x.User2.IsDeleted == false ||
                x.User2Id.Equals(userId) && x.User1.IsDeleted == false)
            .Select(x =>
            x.User1Id.Equals(userId)
                ? new User
                {
                    Id = x.User2.Id,
                    UserName = x.User2.UserName,
                    Status = x.User2.Status,
                    ProfilePicture = x.User2.ProfilePicture
                }
                : new User
                {
                    Id = x.User1.Id,
                    Status = x.User1.Status,
                    UserName = x.User1.UserName,
                    ProfilePicture = x.User2.ProfilePicture
                }).ToListAsync();

        return friends;
    }

    public async Task<Friendship?> GetFriendshipByUser1IdAndUser2IdAsync(Guid user1Id, Guid user2Id, bool trackChanges)
        => await FindByCondition(x => x.User1Id.Equals(user1Id) && x.User2Id.Equals(user2Id), trackChanges).FirstOrDefaultAsync();

    public async Task<IEnumerable<Friendship>> GetFriendUsersMinimalInformation(Guid userId, bool trackChanges)
    {
        var x = await FindByCondition(x =>
            (x.User1Id.Equals(userId) && x.User2.IsDeleted == false && x.User2.Status == Entities.Enums.UserStatus.Online) ||
            (x.User2Id.Equals(userId) && x.User1.IsDeleted == false && x.User1.Status == Entities.Enums.UserStatus.Online), trackChanges: false)
            .Include(x => x.User1)
            .Include(x => x.User2)
            .ToListAsync();

        return x;
    }


    public void CreateFriendship(Friendship friendship)
        => Create(friendship);

    public void DeleteFriendship(Friendship friendship)
        => Delete(friendship);

    public async Task<bool> FriendshipExistsAsync(Guid user1Id, Guid user2Id)
    {
        if (
            await _context.Friendships.AsNoTracking().AnyAsync(x =>
            (x.User1Id.Equals(user1Id) && x.User2Id.Equals(user2Id)) ||
            (x.User1Id.Equals(user2Id) && x.User2Id.Equals(user1Id)))
        )
            return true;

        return false;
    }

    public async Task<int> GetTotalFriendsCountByUserIdAsync(Guid userId)
        => await _context.Friendships.CountAsync(x => x.User1Id.Equals(userId) && x.User2.IsDeleted == false || x.User2Id.Equals(userId) && x.User1.IsDeleted == false);
}