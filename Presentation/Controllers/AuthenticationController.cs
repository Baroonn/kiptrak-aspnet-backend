using Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Service.DTOs;
using Service.ServiceContracts;

namespace Presentation.Controllers;

[ApiVersion("1.0")]
[Route("api/{v:apiversion}/auth")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly IServiceManager _service;
    public AuthenticationController(IServiceManager service)
    {
        _service = service;
    }

    [HttpGet("validatecode")]
    public async Task<IActionResult> ConfirmEmail(string code, string email, string username)
    {
        if(email == null || code == null || username == null){
            var errors = new Dictionary<string, List<string>>();
            errors["Email, Code and Username must be provided"] = new List<string>(){"Email,Code or Username was not provided"};
            return BadRequest(
                errors
            );
        }
        if(!(await _service.AuthenticationService.ConfirmEmail(code, email, username) ))
        {
            return Ok(new { Validated = false});
        }
        return Ok(new { Validated = true });;
    }

    // [HttpGet("confirmuservalid")]
    // public async Task<IActionResult> ConfirmUser(string username)
    // {
    //     if(username == null){
    //         var errors = new Dictionary<string, List<string>>();
    //         errors["Email must be provided"] = new List<string>(){"Email was not provided"};
    //         return BadRequest(
    //             errors
    //         );
    //     }

    //     return Ok(new {Confirmed = await _service.AuthenticationService.IsEmailConfirmedAsync(username)});
    // }


    // [HttpGet("getconfirmationtoken")]
    // public async Task<IActionResult> GetConfirmationToken(string username)
    // {
    //     return Ok(new { Valid = await _service.AuthenticationService.GenerateToken(username) });
    // }
    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser([FromBody] UserRegisterDto userRegisterDto)
    {
        
        if (userRegisterDto is null)
        {
            throw new UserNullException();
        }
        if (!ModelState.IsValid)
        {
            return UnprocessableEntity(ModelState);
        }
        var result = await _service.AuthenticationService.RegisterUser(userRegisterDto);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.TryAddModelError(error.Code, error.Description);
            }
            return BadRequest(ModelState);
        }
        return StatusCode(201);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Authenticate([FromBody] UserAuthenticateDto user) 
    { 
        if (user is null)
        {
            throw new UserNullException();
        }
        if (!ModelState.IsValid)
        {
            return UnprocessableEntity(ModelState);
        }
        if (!await _service.AuthenticationService.ValidateUser(user)) 
            return Unauthorized(); 

        if(!await _service.AuthenticationService.IsUserConfirmedAsync(user.UserName)){
            return Ok(new {Token = await _service.AuthenticationService.GenerateToken(user.UserName)});
        }
        return Ok(new { Token = await _service.AuthenticationService.CreateToken() }); 
    }
}
