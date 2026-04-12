using Entities.Models;

namespace Contracts;

public interface IInterestsRepository
{
    Task<IEnumerable<Interest>> GetAllInterestsAsync(bool trackChanges);
}