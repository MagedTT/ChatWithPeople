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
    public ServiceManager(UserManager<User> userManager,
        RoleManager<IdentityRole<Guid>> roleManager,
        IOptionsMonitor<JwtConfiguration> optionsMonitorJwtConfiguration,
        IMapper mapper,
        IRepositoryManager repositoryManager,
        ILoggerManager loggerManager)
    {
        _authenticationService = new Lazy<IAuthenticationService>(() => new AuthenticationService(userManager, roleManager, optionsMonitorJwtConfiguration, mapper, repositoryManager, loggerManager));
        _userService = new Lazy<IUserService>(() => new UserService(repositoryManager, mapper, loggerManager));
    }
    public IAuthenticationService AuthenticationService => _authenticationService.Value;
    public IUserService UserService => _userService.Value;
}