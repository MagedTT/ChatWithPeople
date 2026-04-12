using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository;

public class InterestsRepository : RepositoryBase<Interest>, IInterestsRepository
{
    public InterestsRepository(ApplicationDbContext context)
        : base(context)
    { }

    public async Task<IEnumerable<Interest>> GetAllInterestsAsync(bool trackChanges)
        => await FindAll(trackChanges).ToListAsync();
}