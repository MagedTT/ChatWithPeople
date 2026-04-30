using Shared.DTOs;
using Shared.RequestFeatures;

namespace Service.Contracts;

public interface IUserService
{
    Task<UserDto?> GetUserByIdAsync(Guid userId, bool trackChanges);
    Task<(IEnumerable<UserDto> users, MetaData metaData)> GetAllUsersAsync(UserParameters userParameters, bool trackChanges);
    Task<string?> GetProfilePictureByUserIdAsync(Guid userId);
    Task<(IEnumerable<UserForDiscoverDTO> usersForDiscoverDto, MetaData metaData)> GetAllUsersForDiscoverWithInterestsAsync(Guid userId, UserParameters userParameters, bool trackChanges);
}