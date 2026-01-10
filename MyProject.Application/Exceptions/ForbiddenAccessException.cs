namespace MyProject.Application.Exceptions;

public sealed class ForbiddenAccessException : ApplicationException
{
    public ForbiddenAccessException() : base("Access to this resource is forbidden.") { }
}

