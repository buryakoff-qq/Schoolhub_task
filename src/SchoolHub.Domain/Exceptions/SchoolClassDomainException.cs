namespace SchoolHub.Domain.Exceptions;

public abstract class SchoolClassDomainException : Exception
{
    protected SchoolClassDomainException(string message) : base(message)
    {
    }
}

public sealed class ClassNameCannotBeEmptyException : SchoolClassDomainException
{
    public ClassNameCannotBeEmptyException() : base("Class name cannot be empty")
    {
    }
}

public sealed class TeacherCannotBeEmptyException : SchoolClassDomainException
{
    public TeacherCannotBeEmptyException() : base("Teacher cannot be empty")
    {
    }
}

public sealed class MaximumStudentsReachedException : SchoolClassDomainException
{
    public MaximumStudentsReachedException() : base("Maximum students reached")
    {
    }
}

public sealed class StudentAlreadyAssignedToClassException : SchoolClassDomainException
{
    public StudentAlreadyAssignedToClassException() : base("Student already assigned to this class")
    {
    }
}

