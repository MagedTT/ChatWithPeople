using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository;

public class UserRepository : RepositoryBase<User>, IUserRepository
{
    public UserRepository(ApplicationDbContext context)
        : base(context)
    { }

    public async Task<User?> GetUserWithUserInterestsByIdAsync(Guid userId, bool trackChanges)
        => await FindByCondition(x => x.Id == userId, trackChanges).Include(x => x.UserInterests).FirstOrDefaultAsync();
}