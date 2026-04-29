using System.Reflection.Metadata;
using Entities.Models;
using Shared.RequestFeatures;

namespace Contracts;

public interface IUserRepository
{
    Task<User?> GetUserByIdAsync(Guid userId, bool trackChanges);
    Task<PagedList<User>> GetAllUsersWithInterestsAsync(UserParameters userParameters, bool trackChanges);
    Task<PagedList<User>> GetAllUsersForDiscoverWithInterestsAsync(Guid userId, UserParameters userParameters, bool trackChanges);
    Task<User?> GetUserWithUserInterestsByIdAsync(Guid userId, bool trackChanges);
    Task<PagedList<User>> GetAllUsersAsync(UserParameters userParameters, bool trackChanges);
}