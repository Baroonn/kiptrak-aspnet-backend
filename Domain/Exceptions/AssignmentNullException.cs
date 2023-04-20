namespace Domain.Exceptions;

public sealed class AssignmentNullException : BadRequestException 
{ 
    public AssignmentNullException() 
        :base ("AssignmentCreateDto is null") 
    { } 
}