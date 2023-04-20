namespace Service.DTOs;

//public record AssignmentReadDto(Guid Id, string Title, string Description, string Notes, string Course, string TeacherName, DateTime DateDue,DateTime? CreatedAt);

public class AssignmentReadDto
{
    public Guid Id { get; set; }
    public string? Title { get; set; }

    public string? Description { get; set; }
    public string? Notes { get; set; }
    public string? Course { get; set; }
    public string? TeacherName { get; set; }
    public DateTime DateDue { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? Username { get; set; }

}