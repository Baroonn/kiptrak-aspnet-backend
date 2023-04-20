namespace Domain.Exceptions;

public sealed class AssignmentNotFoundException : NotFoundException 
{ 
    public AssignmentNotFoundException(Guid assignmentId) 
        :base ($"The assignment with id: {assignmentId} doesn't exist in the database.") 
    { } 
}