namespace SchoolHub.Domain.Exceptions;

public abstract class StudentDomainException : Exception
{
    protected StudentDomainException(string message) : base(message)
    {
    }
}

public sealed class StudentIdCannotBeEmptyException : StudentDomainException
{
    public StudentIdCannotBeEmptyException() : base("Student ID cannot be empty")
    {
    }
}

public sealed class FirstNameCannotBeEmptyException : StudentDomainException
{
    public FirstNameCannotBeEmptyException() : base("First name cannot be empty")
    {
    }
}

public sealed class LastNameCannotBeEmptyException : StudentDomainException
{
    public LastNameCannotBeEmptyException() : base("Last name cannot be empty")
    {
    }
}

