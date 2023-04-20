using System.Security.Claims;
using Domain.Exceptions;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Service.RequestFeatures;
using Service.ServiceContracts;

namespace Presentation.Controllers;

[ApiVersion("1.0")]
[Route("api/{v:apiversion}/users")]
[ApiController]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IServiceManager _service;
    private readonly UserManager<AppUser> _userManager;

    public UsersController(IServiceManager service, UserManager<AppUser> userManager)
    {
        _service = service;
        _userManager = userManager;
    }

    
    [HttpGet]
    public async Task<IActionResult> GetUsers([FromQuery] UserParameters userParameters)
    {
        var users = await _service.UserService.GetAllUsersAsync(userParameters, trackChanges: false);
        return Ok(users);
    }

    [HttpGet("following")]
    public async Task<IActionResult> GetUsersFollowing()
    { 
        var name = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
        var currUser = await _userManager.FindByNameAsync(name);
        var users = await _service.UserService.GetUserFollowingAsync(new Guid(currUser.Id), trackChanges: false);
        return Ok(users);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetUser(Guid id)
    {
        var users = await _service.UserService.GetUserAsync(id, trackChanges: false);
        return Ok(users);
    }

    [HttpGet("{id:guid}/follow")]
    public async Task<IActionResult> FollowUser(Guid id)
    {
        var name = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
        var currUser = await _userManager.FindByNameAsync(name);
        if(currUser is null)
        {
            throw new UserNullException();
        }
        await _service.UserService.FollowUserAsync(id, currUser.Id);
        return NoContent();
    }

    [HttpGet("{id:guid}/unfollow")]
    public async Task<IActionResult> UnfollowUser(Guid id)
    { 
        var name = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
        var currUser = await _userManager.FindByNameAsync(name);
        if(currUser is null)
        {
            throw new UserNullException();
        }
        await _service.UserService.UnfollowUserAsync(id, currUser.Id);
        return NoContent();
    } 
}