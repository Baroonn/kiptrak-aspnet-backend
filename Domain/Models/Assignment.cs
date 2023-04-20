using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models;

public class Assignment
{
    [Column("AssignmentId")]
    public Guid Id { get; set; }
    [Required(ErrorMessage ="Title is required.")]
    public string? Title { get; set; }
    [Required(ErrorMessage = "Description is required.")]
    public string? Description { get; set; }
    public string? Notes{ get; set; }
    [Required(ErrorMessage ="Course is required.")]
    public string? Course { get; set; }
    [Required(ErrorMessage ="TeacherName is required")]
    public string? TeacherName { get; set; }
    public DateTime DateDue { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public string UserId { get; set; }
    public AppUser User { get; set; }
}