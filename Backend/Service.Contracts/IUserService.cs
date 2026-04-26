using Shared.DTOs;
using Shared.RequestFeatures;

namespace Service.Contracts;

public interface IUserService
{
    Task<(IEnumerable<UserDto> users, MetaData metaData)> GetAllUsersAsync(UserParameters userParameters, bool trackChanges);

    Task<(IEnumerable<UserForDiscoverDTO> usersForDiscoverDto, MetaData metaData)> GetAllUsersForDiscoverWithInterestsAsync(UserParameters userParameters, bool trackChanges);
}