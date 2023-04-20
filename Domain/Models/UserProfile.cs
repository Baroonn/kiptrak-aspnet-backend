using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models;

public class UserProfile
{
    public int UserProfileId { get; set; }
    public string? AboutUser { get; set; }
    public string? ProfilePicture{ get; set; }
    public string? Following { get; set; }
    public string? VCode { get; set; }

    public string UserId { get; set; }
    public AppUser User { get; set; }
}