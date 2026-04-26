using System.Reflection.Metadata;
using Entities.Models;
using Shared.RequestFeatures;

namespace Contracts;

public interface IUserRepository
{
    Task<PagedList<User>> GetAllUsersWithInterestsAsync(UserParameters userParameters, bool trackChanges);
    Task<PagedList<User>> GetAllUsersForDiscoverWithInterestsAsync(UserParameters userParameters, bool trackChanges);
    Task<User?> GetUserWithUserInterestsByIdAsync(Guid userId, bool trackChanges);
    Task<PagedList<User>> GetAllUsersAsync(UserParameters userParameters, bool trackChanges);
}