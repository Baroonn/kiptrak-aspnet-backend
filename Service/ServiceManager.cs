using AutoMapper;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Service.Contracts;
using Service.ServiceContracts;

namespace Service;

public sealed class ServiceManager : IServiceManager
{
    private readonly Lazy<IAssignmentService> _assignmentService;
    private readonly Lazy<IAuthenticationService> _authenticationService;
    private readonly Lazy<IUserService> _userService;

    public ServiceManager(IRepoManager repoManager, IMapper mapper, UserManager<AppUser> userManager, IConfiguration configuration, IMailService mailService)
    {
        _assignmentService = new Lazy<IAssignmentService>(() => new AssignmentService(repoManager, mapper));
        _authenticationService = new Lazy<IAuthenticationService>(() => new AuthenticationService(mapper, repoManager, userManager, configuration, mailService));
        _userService = new Lazy<IUserService>(() => new UserService(userManager, mapper, repoManager));
    }
    public IAssignmentService AssignmentService => _assignmentService.Value;

    public IAuthenticationService AuthenticationService => _authenticationService.Value;

    public IUserService UserService => _userService.Value;
}