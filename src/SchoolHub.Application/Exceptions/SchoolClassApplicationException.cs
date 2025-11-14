namespace SchoolHub.Application.Exceptions;

public abstract class SchoolClassApplicationException : Exception
{
    protected SchoolClassApplicationException(string message) : base(message)
    {
    }
}

public sealed class ClassNotFoundException : SchoolClassApplicationException
{
    public ClassNotFoundException() : base("Class not found")
    {
    }
}

public sealed class StudentAlreadyAssignedException : SchoolClassApplicationException
{
    public StudentAlreadyAssignedException() : base("Student already assigned")
    {
    }
}

