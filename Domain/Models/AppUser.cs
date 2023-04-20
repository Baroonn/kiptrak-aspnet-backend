using Microsoft.AspNetCore.Identity;

namespace Domain.Models;

public class AppUser : IdentityUser
{
    public ICollection<Assignment>? Assignments { get; set; }
    public UserProfile Profile { get; set; }
}