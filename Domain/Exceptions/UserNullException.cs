namespace Domain.Exceptions;

public sealed class UserNullException : BadRequestException 
{ 
    public UserNullException() 
        :base ("User was not found: Check your email or username") 
    { } 
}