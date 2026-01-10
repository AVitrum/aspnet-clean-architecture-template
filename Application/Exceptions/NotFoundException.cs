namespace Application.Exceptions;

public sealed class NotFoundException : ApplicationException
{
    public NotFoundException(string entityName, object key) 
        : base($"Entity \"{entityName}\" with key ({key}) was not found.") { }
}

