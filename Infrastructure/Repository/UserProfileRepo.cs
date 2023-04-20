using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Service.Contracts;

namespace Infrastructure.Repository;

public class UserProfileRepo : RepositoryBase<UserProfile>, IUserProfileRepo
{
    public UserProfileRepo(RepositoryContext context)
        :base(context)
        {
            
        }
    public void CreateUserProfile(UserProfile profile)
    {
        Create(profile);
    }

    public async Task<UserProfile> GetUserProfileAsync(Guid userId, bool trackChanges)
    {
        return await FindByCondition(c => c.UserId.Equals(userId.ToString()), trackChanges).SingleOrDefaultAsync();
    }

    public void UpdateUserProfile(UserProfile profile)
    {
        Update(profile);
    }
}