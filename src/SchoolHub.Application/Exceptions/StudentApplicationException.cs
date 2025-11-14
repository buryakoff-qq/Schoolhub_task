namespace SchoolHub.Application.Exceptions;

public abstract class StudentApplicationException : Exception
{
    protected StudentApplicationException(string message) : base(message)
    {
    }
}

public sealed class StudentNotFoundException : StudentApplicationException
{
    public StudentNotFoundException() : base("Student not found")
    {
    }
}

public sealed class StudentIdMustBeUniqueException : StudentApplicationException
{
    public StudentIdMustBeUniqueException() : base("Student ID must be unique")
    {
    }
}

