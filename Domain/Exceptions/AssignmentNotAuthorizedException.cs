namespace Domain.Exceptions;

public sealed class AssignmentNotAuthorizedException : UnauthorizedException 
{ 
    public AssignmentNotAuthorizedException() 
        :base ("You are not authorized to perform this action") 
    { } 
}