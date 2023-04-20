using Domain.Models;

namespace Service.Contracts;

public interface IUserProfileRepo
{
    void CreateUserProfile(UserProfile profile);
    void UpdateUserProfile(UserProfile profile);
    Task<UserProfile> GetUserProfileAsync(Guid userId, bool trackChanges);
}