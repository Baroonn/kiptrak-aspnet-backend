namespace Service.Contracts;

public interface IRepoManager
{
    IAssignmentRepo Assignment { get; }
    IUserProfileRepo Profile { get; }
    Task SaveAsync();
}