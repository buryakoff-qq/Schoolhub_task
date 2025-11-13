namespace SchoolHub.Domain.ValueObjects;

public sealed class StudentId
{
    public string Value { get; }
    
    private StudentId() { }

    public StudentId(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidOperationException("Student ID cannot be empty");
        Value = value.Trim();
    }
}