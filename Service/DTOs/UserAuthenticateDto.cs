using System.ComponentModel.DataAnnotations;

namespace Service.DTOs;

public record UserAuthenticateDto 
{ 
    [Required(ErrorMessage = "User name is required")] 
    public string? UserName { get; init; } 
    [Required(ErrorMessage = "Password name is required")] 
    public string? Password { get; init; } 
}