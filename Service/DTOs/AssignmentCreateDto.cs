using System.ComponentModel.DataAnnotations;

namespace Service.DTOs;
public record AssignmentCreateDto : IValidatableObject
{
    [Required(ErrorMessage = "Title is required")]
    public string? Title { get; set; }

    [Required(ErrorMessage = "Description is required")]
    public string? Description { get; set; }
    public string? Notes { get; set; }

    [Required(ErrorMessage = "Course is required")]
    public string? Course { get; set; }

    [Required(ErrorMessage ="TeacherName is required")]
    public string? TeacherName { get; set; }
    [Required(ErrorMessage ="DateDue is required")]
    public DateTime DateDue { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var errorMessage = "Please provide future date";
        if(DateTime.Now > DateDue)
        {
            yield return new ValidationResult(errorMessage, new[] { nameof(DateDue)});
        }
    }
}