using SchoolHub.Domain.Entities;
using SchoolHub.Domain.Exceptions;

namespace SchoolHub.Domain.Tests.Entities;

public class SchoolClassTests
{
    [Fact]
    public void Create_WithValidData_ShouldCreateClass()
    {
        // Act
        var schoolClass = new SchoolClass("Math", "Mr. Smith");

        // Assert
        Assert.NotNull(schoolClass);
        Assert.Equal("Math", schoolClass.Name);
        Assert.Equal("Mr. Smith", schoolClass.Teacher);
        Assert.Empty(schoolClass.StudentIds);
    }

    [Fact]
    public void Create_WithEmptyName_ShouldThrowException()
    {
        // Act & Assert
        Assert.Throws<ClassNameCannotBeEmptyException>(() =>
            new SchoolClass("", "Mr. Smith"));
    }

    [Fact]
    public void Create_WithWhitespaceName_ShouldThrowException()
    {
        // Act & Assert
        Assert.Throws<ClassNameCannotBeEmptyException>(() =>
            new SchoolClass("   ", "Mr. Smith"));
    }

    [Fact]
    public void Create_WithEmptyTeacher_ShouldThrowException()
    {
        // Act & Assert
        Assert.Throws<TeacherCannotBeEmptyException>(() =>
            new SchoolClass("Math", ""));
    }

    [Fact]
    public void Update_WithValidData_ShouldUpdateClass()
    {
        // Arrange
        var schoolClass = new SchoolClass("Math", "Mr. Smith");

        // Act
        schoolClass.Update("Science", "Ms. Johnson");

        // Assert
        Assert.Equal("Science", schoolClass.Name);
        Assert.Equal("Ms. Johnson", schoolClass.Teacher);
    }

    [Fact]
    public void Update_WithTrimmedValues_ShouldTrimWhitespace()
    {
        // Arrange
        var schoolClass = new SchoolClass("Math", "Mr. Smith");

        // Act
        schoolClass.Update("  Science  ", "  Ms. Johnson  ");

        // Assert
        Assert.Equal("Science", schoolClass.Name);
        Assert.Equal("Ms. Johnson", schoolClass.Teacher);
    }

    [Fact]
    public void AddStudent_WithValidStudentId_ShouldAddStudent()
    {
        // Arrange
        var schoolClass = new SchoolClass("Math", "Mr. Smith");
        var studentId = Guid.NewGuid();

        // Act
        schoolClass.AddStudent(studentId);

        // Assert
        Assert.Single(schoolClass.StudentIds);
        Assert.Contains(studentId, schoolClass.StudentIds);
    }

    [Fact]
    public void AddStudent_WithMaxStudents_ShouldThrowException()
    {
        // Arrange
        var schoolClass = new SchoolClass("Math", "Mr. Smith");
        
        // Add 20 students
        for (int i = 0; i < SchoolClass.MaxStudents; i++)
        {
            schoolClass.AddStudent(Guid.NewGuid());
        }

        // Act & Assert
        Assert.Throws<MaximumStudentsReachedException>(() =>
            schoolClass.AddStudent(Guid.NewGuid()));
    }

    [Fact]
    public void AddStudent_WithDuplicateStudentId_ShouldThrowException()
    {
        // Arrange
        var schoolClass = new SchoolClass("Math", "Mr. Smith");
        var studentId = Guid.NewGuid();
        schoolClass.AddStudent(studentId);

        // Act & Assert
        Assert.Throws<StudentAlreadyAssignedToClassException>(() =>
            schoolClass.AddStudent(studentId));
    }

    [Fact]
    public void RemoveStudent_WithExistingStudent_ShouldRemoveStudent()
    {
        // Arrange
        var schoolClass = new SchoolClass("Math", "Mr. Smith");
        var studentId = Guid.NewGuid();
        schoolClass.AddStudent(studentId);

        // Act
        schoolClass.RemoveStudent(studentId);

        // Assert
        Assert.Empty(schoolClass.StudentIds);
    }
}

