using Service.Contracts;

namespace Infrastructure.Repository;

public sealed class RepoManager : IRepoManager
{
    private readonly RepositoryContext _repositoryContext;
    private readonly Lazy<IAssignmentRepo> _assignmentRepo;
    private readonly Lazy<IUserProfileRepo> _profileRepo;
    public RepoManager(RepositoryContext repositoryContext)
    {
        _repositoryContext = repositoryContext;
        _assignmentRepo = new Lazy<IAssignmentRepo>(() => new AssignmentRepo(repositoryContext));
        _profileRepo = new Lazy<IUserProfileRepo>(() => new UserProfileRepo(repositoryContext));
    }
    public IAssignmentRepo Assignment => _assignmentRepo.Value;

    public IUserProfileRepo Profile => _profileRepo.Value;

    public async Task SaveAsync()
    {
        await _repositoryContext.SaveChangesAsync();
    }
}