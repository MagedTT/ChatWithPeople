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
    public ServiceManager(UserManager<User> userManager,
        RoleManager<IdentityRole<Guid>> roleManager,
        IOptionsMonitor<JwtConfiguration> optionsMonitorJwtConfiguration,
        IMapper mapper,
        IRepositoryManager repositoryManager,
        ILoggerManager loggerManager)
    {
        _authenticationService = new Lazy<IAuthenticationService>(() => new AuthenticationService(userManager, roleManager, optionsMonitorJwtConfiguration, mapper, repositoryManager, loggerManager));
    }
    public IAuthenticationService AuthenticationService => _authenticationService.Value;
}