using SchoolHub.Domain.Exceptions;

namespace SchoolHub.Domain.Entities;

public sealed class SchoolClass
{
    public const int MaxStudents = 20;
    
    public Guid Id { get; private set; }
    public string Name { get; private set; } = null!;
    public string Teacher { get; private set; } = null!;

    private readonly List<Guid> _studentIds = new();
    public IReadOnlyList<Guid> StudentIds => _studentIds;

    private SchoolClass() { }

    public SchoolClass(string name, string teacher)
    {
        Id = Guid.NewGuid();
        Update(name, teacher);
    }

    public void Update(string name, string teacher)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ClassNameCannotBeEmptyException();
        if (string.IsNullOrWhiteSpace(teacher))
            throw new TeacherCannotBeEmptyException();
        
        Name = name.Trim();
        Teacher = teacher.Trim();
    }

    public void AddStudent(Guid studentId)
    {
        if (_studentIds.Count >= MaxStudents)
            throw new MaximumStudentsReachedException();
        if (_studentIds.Contains(studentId))
            throw new StudentAlreadyAssignedToClassException();
        
        _studentIds.Add(studentId);
    }

    public void RemoveStudent(Guid studentId)
    {
        _studentIds.Remove(studentId);
    }
}