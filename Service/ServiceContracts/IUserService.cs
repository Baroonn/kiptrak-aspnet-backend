using Domain.Models;
using Service.DTOs;
using Service.RequestFeatures;

namespace Service.ServiceContracts;

public interface IUserService
{
    Task<IEnumerable<UserReadDto>> GetAllUsersAsync(UserParameters userParameters, bool trackChanges);
    Task<IEnumerable<UserReadDto>> GetUserFollowingAsync(Guid userId, bool trackChanges);
    Task<UserProfileReadDto> GetUserAsync(Guid userId, bool trackChanges);
    Task FollowUserAsync(Guid userId, string currentUserId);
    Task UnfollowUserAsync(Guid userId, string currentUserId);   
}