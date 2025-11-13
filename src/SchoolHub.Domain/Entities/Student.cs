using SchoolHub.Domain.ValueObjects;

namespace SchoolHub.Domain.Entities;

public sealed class Student
{
    public Guid Id { get; private set; }
    public StudentId StudentId { get; private set; }
    public string FirstName { get; private set; } = null!;
    public string LastName { get; private set; } = null!;
    public DateOnly BirthDate { get; private set; }

    public Address Address { get; private set; } = null!;
    
    private Student() { }

    public Student (StudentId studentId, string firstName, string lastName, DateOnly birthDate, Address address)
    {
        Id = Guid.NewGuid();
        Update(studentId, firstName, lastName, birthDate, address);
    }

    public void Update(StudentId studentId, string firstName, string lastName, DateOnly birthDate, Address address)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new InvalidOperationException("First name cannot empty.");
        if (string.IsNullOrWhiteSpace(lastName))
            throw new InvalidOperationException("Last name cannot empty.");
        
        StudentId = studentId ?? throw new InvalidOperationException("StudentId cannot be null");
        FirstName = firstName.Trim();
        LastName = lastName.Trim();
        BirthDate = birthDate;

        Address = address;
    }
}