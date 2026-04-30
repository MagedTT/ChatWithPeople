using System.Security.Cryptography.X509Certificates;
using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Shared.RequestFeatures;

namespace Repository;

public class UserRepository : RepositoryBase<User>, IUserRepository
{
    public UserRepository(ApplicationDbContext context)
        : base(context)
    { }

    public async Task<User?> GetUserByIdAsync(Guid userId, bool trackChanges)
        => await FindByCondition(x => x.Id.Equals(userId), trackChanges).FirstOrDefaultAsync();

    public Task<PagedList<User>> GetAllUsersWithInterestsAsync(UserParameters userParameters, bool trackChanges)
    {
        throw new NotImplementedException();
    }

    public async Task<User?> GetUserWithUserInterestsByIdAsync(Guid userId, bool trackChanges)
        => await FindByCondition(x => x.Id == userId, trackChanges).Include(x => x.UserInterests).FirstOrDefaultAsync();

    public async Task<string?> GetProfilePictureByUserIdAsync(Guid userId)
        => await _context.Users.Where(x => x.Id.Equals(userId)).Select(x => x.ProfilePicture != null ? Convert.ToBase64String(x.ProfilePicture) : "").FirstOrDefaultAsync();

    public async Task<PagedList<User>> GetAllUsersAsync(UserParameters userParameters, bool trackChanges)
    {
        List<User> users =
            await FindAll(trackChanges)
            .Skip((userParameters.PageNumber - 1) * userParameters.PageSize)
            .Take(userParameters.PageSize).ToListAsync();

        int count = await _context.Users.CountAsync();

        return new PagedList<User>(users, count, userParameters.PageNumber, userParameters.PageSize);
    }

    public async Task<PagedList<User>> GetAllUsersForDiscoverWithInterestsAsync(Guid userId, UserParameters userParameters, bool trackChanges)
    {
        List<User> users =
            await FindByCondition(x => !x.Id.Equals(userId), trackChanges)
            .Skip((userParameters.PageNumber - 1) * userParameters.PageSize)
            .Take(userParameters.PageSize)
            .Include(x => x.UserInterests)
            .ThenInclude(x => x.Interest)
            .ToListAsync();

        int count = await _context.Users.CountAsync();

        return new PagedList<User>(users, count, userParameters.PageNumber, userParameters.PageSize);
    }
}