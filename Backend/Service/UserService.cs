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
        PagedList<User> usersWithMetaData = await _repositoryManager.UserRepository.GetAllUsersAsync(userParameters, trackChanges);

        IEnumerable<UserDto> userDtos = _mapper.Map<IEnumerable<UserDto>>(usersWithMetaData);

        return (users: userDtos, metaData: usersWithMetaData.MetaData);
    }
}