using System.ComponentModel.DataAnnotations;

namespace Service.DTOs;

public record UserRegisterDto
{ 
    [Required(ErrorMessage = "Username is required")] 
    public string? UserName { get; init; } 
    [Required(ErrorMessage = "Password is required")] 
    public string? Password { get; init; } 
    public string? Email { get; init; } 
    public string? PhoneNumber { get; init; } 
}