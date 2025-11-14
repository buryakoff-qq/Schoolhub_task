using SchoolHub.Domain.Exceptions;
using SchoolHub.Domain.ValueObjects;

namespace SchoolHub.Domain.Tests.ValueObjects;

public class StudentIdTests
{
    [Fact]
    public void Create_WithValidValue_ShouldCreateStudentId()
    {
        // Act
        var studentId = new StudentId("STU001");

        // Assert
        Assert.NotNull(studentId);
        Assert.Equal("STU001", studentId.Value);
    }

    [Fact]
    public void Create_WithWhitespace_ShouldTrimValue()
    {
        // Act
        var studentId = new StudentId("  STU002  ");

        // Assert
        Assert.Equal("STU002", studentId.Value);
    }

    [Fact]
    public void Create_WithEmptyString_ShouldThrowException()
    {
        // Act & Assert
        Assert.Throws<StudentIdCannotBeEmptyException>(() =>
            new StudentId(""));
    }

    [Fact]
    public void Create_WithWhitespaceOnly_ShouldThrowException()
    {
        // Act & Assert
        Assert.Throws<StudentIdCannotBeEmptyException>(() =>
            new StudentId("   "));
    }

    [Fact]
    public void Create_WithNull_ShouldThrowException()
    {
        // Act & Assert
        Assert.Throws<StudentIdCannotBeEmptyException>(() =>
            new StudentId(null!));
    }
}

