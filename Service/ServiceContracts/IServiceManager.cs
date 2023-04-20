namespace Service.ServiceContracts;

public interface IServiceManager
{
    IAssignmentService AssignmentService { get; }
    IAuthenticationService AuthenticationService { get; }
    IUserService UserService { get; }
}