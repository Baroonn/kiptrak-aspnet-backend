using Microsoft.AspNetCore.Identity;
using Service.DTOs;

namespace Service.ServiceContracts;

public interface IAuthenticationService 
{ 
    Task<IdentityResult> RegisterUser(UserRegisterDto user); 
    Task<bool> ValidateUser(UserAuthenticateDto userForAuth);
    Task<bool> ConfirmEmail(string token, string email, string username);
    Task<bool> GenerateToken(string email);
    Task<string> CreateToken();
    Task<bool> IsUserConfirmedAsync(string email);
}