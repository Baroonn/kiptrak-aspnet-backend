using Domain.Models;

namespace Service.DTOs;

public record UserProfileReadDto(string UserId, string ProfilePicture, string AboutUser);