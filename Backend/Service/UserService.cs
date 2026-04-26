using AutoMapper;
using Contracts;
using Entities.Models;
using Service.Contracts;
using Shared.DTOs;
using Shared.RequestFeatures;

namespace Service;

public class UserService : IUserService
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _mapper;
    private readonly ILoggerManager _logger;

    public UserService(IRepositoryManager repositoryManager, IMapper mapper, ILoggerManager logger)
    {
        _repositoryManager = repositoryManager;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<(IEnumerable<UserDto> users, MetaData metaData)> GetAllUsersAsync(UserParameters userParameters, bool trackChanges)
    {
        PagedList<User> usersWithMetaData = await _repositoryManager.UserRepository.GetAllUsersAsync(userParameters, trackChanges: trackChanges);

        IEnumerable<UserDto> userDtos = _mapper.Map<IEnumerable<UserDto>>(usersWithMetaData);

        return (users: userDtos, metaData: usersWithMetaData.MetaData);
    }

    public async Task<(IEnumerable<UserForDiscoverDTO> usersForDiscoverDto, MetaData metaData)> GetAllUsersForDiscoverWithInterestsAsync(UserParameters userParameters, bool trackChanges)
    {
        PagedList<User> usersWithMetaData = await _repositoryManager.UserRepository.GetAllUsersForDiscoverWithInterestsAsync(userParameters, trackChanges: trackChanges);

        IEnumerable<UserForDiscoverDTO> usersForDiscoverDtos = usersWithMetaData.Select(x => new UserForDiscoverDTO
        {
            Id = x.Id,
            UserName = x.UserName ?? "",
            ProfilePicture = x.ProfilePicture != null ? Convert.ToBase64String(x.ProfilePicture) : null,
            Status = x.Status,
            Age = x.Age,
            Interests = x.UserInterests.Select(i => i.Interest.Name).ToList()
        });

        return (usersForDiscoverDto: usersForDiscoverDtos, metaData: usersWithMetaData.MetaData);
    }
}