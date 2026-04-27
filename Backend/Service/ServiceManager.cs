using AutoMapper;
using Contracts;
using Entities.ConfigurationModels;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Service.Contracts;

namespace Service;

public class ServiceManager : IServiceManager
{
    private readonly Lazy<IAuthenticationService> _authenticationService;
    private readonly Lazy<IUserService> _userService;
    private readonly Lazy<IFriendshipsService> _friendshipsService;
    private readonly Lazy<IFriendRequestService> _friendRequestService;
    private readonly Lazy<IGroupService> _groupService;
    private readonly Lazy<IConversationService> _conversationService;
    public ServiceManager(UserManager<User> userManager,
    RoleManager<IdentityRole<Guid>> roleManager,
    IOptionsMonitor<JwtConfiguration> optionsMonitorJwtConfiguration,
    IMapper mapper,
    IRepositoryManager repositoryManager,
    ILoggerManager loggerManager)
    {
        _authenticationService = new Lazy<IAuthenticationService>(() => new AuthenticationService(userManager, roleManager, optionsMonitorJwtConfiguration, mapper, repositoryManager, loggerManager));
        _userService = new Lazy<IUserService>(() => new UserService(repositoryManager, mapper, loggerManager));
        _friendshipsService = new Lazy<IFriendshipsService>(() => new FriendshipsService(userManager, mapper, repositoryManager, loggerManager));
        _friendRequestService = new Lazy<IFriendRequestService>(() => new FriendRequestService(userManager, repositoryManager, loggerManager, mapper));
        _groupService = new Lazy<IGroupService>(() => new GroupService(userManager, repositoryManager, mapper, loggerManager));
        _conversationService = new Lazy<IConversationService>(() => new ConversationService(userManager, repositoryManager, loggerManager, mapper));
    }
    public IAuthenticationService AuthenticationService => _authenticationService.Value;
    public IUserService UserService => _userService.Value;

    public IFriendshipsService FriendshipsService => _friendshipsService.Value;

    public IFriendRequestService FriendRequestService => _friendRequestService.Value;

    public IGroupService GroupService => _groupService.Value;

    public IConversationService ConversationService => _conversationService.Value;
}