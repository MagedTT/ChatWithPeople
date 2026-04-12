using System.Reflection.Metadata;
using Entities.Models;

namespace Contracts;

public interface IUserRepository
{
    Task<User?> GetUserWithUserInterestsByIdAsync(Guid userId, bool trackChanges);
}