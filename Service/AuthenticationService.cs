using AutoMapper;
using Domain.Exceptions;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Service.Contracts;
using Service.DTOs;
using Service.ServiceContracts;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Service;

internal sealed class AuthenticationService : IAuthenticationService
{
    private readonly IMapper _mapper;
    private readonly UserManager<AppUser> _userManager;
    private readonly IConfiguration _configuration;
    private readonly IRepoManager _repoManager;
    private readonly IMailService _mailService;
    private AppUser? _user;
    public AuthenticationService(IMapper mapper, IRepoManager repoManager, UserManager<AppUser> userManager, IConfiguration configuration, IMailService mailService)
    {
        _mapper = mapper;
        _userManager = userManager;
        _configuration = configuration;
        _repoManager = repoManager;
        _mailService = mailService;
    }

    private SigningCredentials GetSigningCredentials()
    {
        var key = Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("SECRET"));
        var secret = new SymmetricSecurityKey(key);
        return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
    }

    private async Task<List<Claim>> GetClaims()
    {
        var claims = new List<Claim> { new Claim(ClaimTypes.Name, _user.UserName) };
        return claims;
    }

    private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var tokenOptions = new JwtSecurityToken(
            issuer: jwtSettings["validIssuer"],
            audience: jwtSettings["validAudience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings["expires"])),
            signingCredentials: signingCredentials
        );
        return tokenOptions;
    }
    public async Task<string> CreateToken()
    {
        var signingCredentials = GetSigningCredentials();
        var claims = await GetClaims();
        var tokenOptions = GenerateTokenOptions(signingCredentials, claims);
        return new JwtSecurityTokenHandler()
        .WriteToken(tokenOptions);
    }

    public async Task<bool> GenerateToken(string username)
    {
        var user = await _userManager.FindByNameAsync(username);
        if (user == null)
        {
            throw new UserNullException();
        }
        if (user.EmailConfirmed)
        {
            return true;
        }
        Random r = new Random();
        string code = r.Next(0, 1000000).ToString("000000");
        var profile = await _repoManager.Profile.GetUserProfileAsync(new Guid(user.Id), true);
        profile.VCode = code;
        _repoManager.Profile.UpdateUserProfile(profile);
        await _repoManager.SaveAsync();
        var emaill = new MailRequest();
        emaill.ToEmail = user.Email;
        emaill.Subject = "KipTrak Email Verification";
        emaill.Body = code;
        await _mailService.SendEmailAsync(emaill);
        return false;
    }

    public async Task<bool> IsUserConfirmedAsync(string username)
    {
        var user = await _userManager.FindByNameAsync(username);
        return user.EmailConfirmed;
    }
    public async Task<bool> ConfirmEmail(string code, string email, string username)
    {
        var user = await _userManager.Users.Where(x => x.UserName == username && x.Email == email).FirstOrDefaultAsync();
        if (user == null)
        {
            throw new UserNullException();
        }
        var profile = await _repoManager.Profile.GetUserProfileAsync(new Guid(user.Id), true);

        if (profile.VCode == code)
        {
            user.EmailConfirmed = true;
            var res = await _userManager.UpdateAsync(user);
            return res.Succeeded;
        }
        else
        {
            return false;
        }

    }
    public async Task<IdentityResult> RegisterUser(UserRegisterDto userRegisterDto)
    {
        var user = _mapper.Map<AppUser>(userRegisterDto);
        var result = await _userManager.CreateAsync(user, userRegisterDto.Password);
        if (result.Succeeded)
        {
            Random r = new Random();
            string code = r.Next(0, 1000000).ToString("000000");
            var validuser = await _userManager.FindByNameAsync(user.UserName);
            UserProfile profile = new UserProfile();
            profile.VCode = code;
            profile.UserId = validuser.Id;
            _repoManager.Profile.CreateUserProfile(profile);
            await _repoManager.SaveAsync();
            //var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var email = new MailRequest();
            email.ToEmail = user.Email;
            email.Subject = "KipTrak Email Verification";
            email.Body = code;
            await _mailService.SendEmailAsync(email);
        }
        return result;
    }

    public async Task<bool> ValidateUser(UserAuthenticateDto userForAuth)
    {
        _user = await _userManager.FindByNameAsync(userForAuth.UserName);
        var result = (_user != null && await _userManager.CheckPasswordAsync(_user, userForAuth.Password));
        return result;
    }


}