using System.Reflection.Metadata;
using Entities.Models;
using Shared.RequestFeatures;

namespace Contracts;

public interface IUserRepository
{
    Task<PagedList<User>> GetAllUsersAsync(UserParameters userParameters, bool trackChanges);
    Task<User?> GetUserWithUserInterestsByIdAsync(Guid userId, bool trackChanges);
}