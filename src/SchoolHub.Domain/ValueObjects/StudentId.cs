using SchoolHub.Domain.Exceptions;

namespace SchoolHub.Domain.ValueObjects;

public sealed class StudentId
{
    public string Value { get; }
    
    private StudentId() { }

    public StudentId(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new StudentIdCannotBeEmptyException();
        Value = value.Trim();
    }
}