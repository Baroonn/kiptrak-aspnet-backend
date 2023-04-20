using AutoMapper;
using Domain.Exceptions;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Service.Contracts;
using Service.DTOs;
using Service.RequestFeatures;
using Service.ServiceContracts;

namespace Service;

internal sealed class UserService : IUserService
{

    private readonly UserManager<AppUser> _userManager;
    private readonly IMapper _mapper;
    private readonly IRepoManager _repoManager;

    public UserService(UserManager<AppUser> userManager, IMapper mapper, IRepoManager repoManager)
    {
        _userManager = userManager;
        _mapper = mapper;
        _repoManager = repoManager;
    }

    public async Task FollowUserAsync(Guid userId, string currentUserId)
    {
        var user = _userManager.Users.Where(x => x.Id == userId.ToString()).FirstOrDefault();
        if (user is null)
        {
            throw new UserNullException();
        }
        var currUser = await _repoManager.Profile.GetUserProfileAsync(new Guid(currentUserId), trackChanges: true);
        if (currUser is null)
        {
            throw new UserNullException();
        }
        if (currUser.Following is null)
        {
            currUser.Following = $"{user.Id}|";
            await _repoManager.SaveAsync();
        }
        else if (!currUser.Following.Contains(user.Id))
        {
            currUser.Following += $"{user.Id}|";
            await _repoManager.SaveAsync();
        }

    }

    public async Task<IEnumerable<UserReadDto>> GetAllUsersAsync(UserParameters userParameters, bool trackChanges)
    {
        List<AppUser>? users = new List<AppUser>();
        if (userParameters != null && !string.IsNullOrEmpty(userParameters.SearchTerm))
        {
            users = await _userManager.Users.Where(u => u.UserName.Contains(userParameters.SearchTerm)).ToListAsync();
        }

        var userReadDto = _mapper.Map<IEnumerable<UserReadDto>>(users);
        return userReadDto;
    }

    public async Task<UserProfileReadDto> GetUserAsync(Guid userId, bool trackChanges)
    {
        var userprofile = await _repoManager.Profile.GetUserProfileAsync(userId, trackChanges);
        var userProfileReadDto = _mapper.Map<UserProfileReadDto>(userprofile);
        return userProfileReadDto;
    }

    public async Task<IEnumerable<UserReadDto>> GetUserFollowingAsync(Guid userId, bool trackChanges)
    {
        var userprofile = await _repoManager.Profile.GetUserProfileAsync(userId, trackChanges);
        var following = userprofile.Following ?? "";
        var users = await _userManager.Users.Where(x => following.Contains(x.Id)).ToListAsync();
        var userReadDto = _mapper.Map<IEnumerable<UserReadDto>>(users);
        return userReadDto;
    }

    public async Task UnfollowUserAsync(Guid userId, string currentUserId)
    {
        var user = _userManager.Users.Where(x => x.Id == userId.ToString()).FirstOrDefault();
        if (user is null)
        {
            throw new UserNullException();
        }
        var userProfile = await _repoManager.Profile.GetUserProfileAsync(new Guid(currentUserId), trackChanges: true);
        if (userProfile is null)
        {
            throw new UserNullException();
        }
        var following = userProfile.Following ?? "";
        if (following.Contains(user.Id))
        {
            userProfile.Following = following.Replace($"{user.Id}|", "");
            await _repoManager.SaveAsync();
        }
    }


}