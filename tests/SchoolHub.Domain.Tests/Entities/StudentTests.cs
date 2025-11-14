using SchoolHub.Domain.Entities;
using SchoolHub.Domain.Exceptions;
using SchoolHub.Domain.ValueObjects;

namespace SchoolHub.Domain.Tests.Entities;

public class StudentTests
{
    [Fact]
    public void Create_WithValidData_ShouldCreateStudent()
    {
        // Arrange
        var studentId = new StudentId("STU001");
        var address = new Address("City", "Street", "12345");
        var birthDate = new DateOnly(2010, 1, 1);

        // Act
        var student = new Student(studentId, "John", "Doe", birthDate, address);

        // Assert
        Assert.NotNull(student);
        Assert.Equal("STU001", student.StudentId.Value);
        Assert.Equal("John", student.FirstName);
        Assert.Equal("Doe", student.LastName);
        Assert.Equal(birthDate, student.BirthDate);
        Assert.Equal("City", student.Address.City);
        Assert.Equal("Street", student.Address.Street);
        Assert.Equal("12345", student.Address.PostalCode);
    }

    [Fact]
    public void Create_WithOptionalAddress_ShouldCreateStudent()
    {
        // Arrange
        var studentId = new StudentId("STU002");
        var address = new Address(null, null, null);
        var birthDate = new DateOnly(2010, 1, 1);

        // Act
        var student = new Student(studentId, "Jane", "Smith", birthDate, address);

        // Assert
        Assert.NotNull(student);
        Assert.Null(student.Address.City);
        Assert.Null(student.Address.Street);
        Assert.Null(student.Address.PostalCode);
    }

    [Fact]
    public void Create_WithEmptyFirstName_ShouldThrowException()
    {
        // Arrange
        var studentId = new StudentId("STU003");
        var address = new Address(null, null, null);
        var birthDate = new DateOnly(2010, 1, 1);

        // Act & Assert
        Assert.Throws<FirstNameCannotBeEmptyException>(() =>
            new Student(studentId, "", "Doe", birthDate, address));
    }

    [Fact]
    public void Create_WithWhitespaceFirstName_ShouldThrowException()
    {
        // Arrange
        var studentId = new StudentId("STU004");
        var address = new Address(null, null, null);
        var birthDate = new DateOnly(2010, 1, 1);

        // Act & Assert
        Assert.Throws<FirstNameCannotBeEmptyException>(() =>
            new Student(studentId, "   ", "Doe", birthDate, address));
    }

    [Fact]
    public void Create_WithEmptyLastName_ShouldThrowException()
    {
        // Arrange
        var studentId = new StudentId("STU005");
        var address = new Address(null, null, null);
        var birthDate = new DateOnly(2010, 1, 1);

        // Act & Assert
        Assert.Throws<LastNameCannotBeEmptyException>(() =>
            new Student(studentId, "John", "", birthDate, address));
    }

    [Fact]
    public void Create_WithNullStudentId_ShouldThrowException()
    {
        // Arrange
        var address = new Address(null, null, null);
        var birthDate = new DateOnly(2010, 1, 1);

        // Act & Assert
        Assert.Throws<StudentIdCannotBeEmptyException>(() =>
            new Student(null!, "John", "Doe", birthDate, address));
    }

    [Fact]
    public void Update_WithValidData_ShouldUpdateStudent()
    {
        // Arrange
        var studentId = new StudentId("STU006");
        var address = new Address("OldCity", "OldStreet", "00000");
        var birthDate = new DateOnly(2010, 1, 1);
        var student = new Student(studentId, "John", "Doe", birthDate, address);

        var newStudentId = new StudentId("STU007");
        var newAddress = new Address("NewCity", "NewStreet", "99999");
        var newBirthDate = new DateOnly(2011, 2, 2);

        // Act
        student.Update(newStudentId, "Jane", "Smith", newBirthDate, newAddress);

        // Assert
        Assert.Equal("STU007", student.StudentId.Value);
        Assert.Equal("Jane", student.FirstName);
        Assert.Equal("Smith", student.LastName);
        Assert.Equal(newBirthDate, student.BirthDate);
        Assert.Equal("NewCity", student.Address.City);
    }
}

