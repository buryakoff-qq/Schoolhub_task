using Moq;
using SchoolHub.Application.DTOs;
using SchoolHub.Application.Exceptions;
using SchoolHub.Application.Interfaces;
using SchoolHub.Application.Services;
using SchoolHub.Domain.Entities;
using SchoolHub.Domain.Exceptions;

namespace SchoolHub.Application.Tests.Services;

public class SchoolClassServiceTests
{
    private readonly Mock<ISchoolClassRepository> _classRepositoryMock;
    private readonly Mock<IStudentRepository> _studentRepositoryMock;
    private readonly SchoolClassService _service;

    public SchoolClassServiceTests()
    {
        _classRepositoryMock = new Mock<ISchoolClassRepository>();
        _studentRepositoryMock = new Mock<IStudentRepository>();
        _service = new SchoolClassService(_classRepositoryMock.Object, _studentRepositoryMock.Object);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllClasses()
    {
        // Arrange
        var classes = new List<SchoolClass>
        {
            new SchoolClass("Math", "Mr. Smith"),
            new SchoolClass("Science", "Ms. Johnson")
        };

        _classRepositoryMock.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(classes);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal("Math", result[0].Name);
        Assert.Equal("Science", result[1].Name);
    }

    [Fact]
    public async Task GetByIdAsync_WithExistingId_ShouldReturnClass()
    {
        // Arrange
        var id = Guid.NewGuid();
        var schoolClass = new SchoolClass("Math", "Mr. Smith");
        SetId(schoolClass, id);

        _classRepositoryMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(schoolClass);

        // Act
        var result = await _service.GetByIdAsync(id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(id, result.Id);
        Assert.Equal("Math", result.Name);
    }

    [Fact]
    public async Task GetByIdAsync_WithNonExistingId_ShouldThrowException()
    {
        // Arrange
        var id = Guid.NewGuid();
        _classRepositoryMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((SchoolClass?)null);

        // Act & Assert
        await Assert.ThrowsAsync<ClassNotFoundException>(() =>
            _service.GetByIdAsync(id));
    }

    [Fact]
    public async Task CreateAsync_WithValidData_ShouldCreateClass()
    {
        // Arrange
        _classRepositoryMock.Setup(r => r.AddAsync(It.IsAny<SchoolClass>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _service.CreateAsync("Math", "Mr. Smith");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Math", result.Name);
        Assert.Equal("Mr. Smith", result.Teacher);
        _classRepositoryMock.Verify(r => r.AddAsync(It.IsAny<SchoolClass>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_WithValidData_ShouldUpdateClass()
    {
        // Arrange
        var id = Guid.NewGuid();
        var schoolClass = new SchoolClass("Math", "Mr. Smith");
        SetId(schoolClass, id);

        _classRepositoryMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(schoolClass);
        _classRepositoryMock.Setup(r => r.UpdateAsync(It.IsAny<SchoolClass>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _service.UpdateAsync(id, "Science", "Ms. Johnson");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Science", result.Name);
        Assert.Equal("Ms. Johnson", result.Teacher);
        _classRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<SchoolClass>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_WithNonExistingId_ShouldThrowException()
    {
        // Arrange
        var id = Guid.NewGuid();
        _classRepositoryMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((SchoolClass?)null);

        // Act & Assert
        await Assert.ThrowsAsync<ClassNotFoundException>(() =>
            _service.UpdateAsync(id, "Science", "Ms. Johnson"));
    }

    [Fact]
    public async Task DeleteAsync_WithExistingId_ShouldDeleteClass()
    {
        // Arrange
        var id = Guid.NewGuid();
        var schoolClass = new SchoolClass("Math", "Mr. Smith");
        SetId(schoolClass, id);

        _classRepositoryMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(schoolClass);
        _classRepositoryMock.Setup(r => r.RemoveAsync(It.IsAny<SchoolClass>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _service.DeleteAsync(id);

        // Assert
        _classRepositoryMock.Verify(r => r.RemoveAsync(It.IsAny<SchoolClass>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_WithNonExistingId_ShouldThrowException()
    {
        // Arrange
        var id = Guid.NewGuid();
        _classRepositoryMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((SchoolClass?)null);

        // Act & Assert
        await Assert.ThrowsAsync<ClassNotFoundException>(() =>
            _service.DeleteAsync(id));
    }

    [Fact]
    public async Task AssignAsync_WithValidData_ShouldAssignStudent()
    {
        // Arrange
        var classId = Guid.NewGuid();
        var studentId = Guid.NewGuid();
        var schoolClass = new SchoolClass("Math", "Mr. Smith");
        SetId(schoolClass, classId);
        var student = CreateTestStudent(studentId);

        _classRepositoryMock.Setup(r => r.GetByIdAsync(classId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(schoolClass);
        _studentRepositoryMock.Setup(r => r.GetByIdAsync(studentId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(student);
        _classRepositoryMock.Setup(r => r.UpdateAsync(It.IsAny<SchoolClass>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _service.AssignAsync(classId, studentId);

        // Assert
        Assert.Single(schoolClass.StudentIds);
        Assert.Contains(studentId, schoolClass.StudentIds);
        _classRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<SchoolClass>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task AssignAsync_WithNonExistingClass_ShouldThrowException()
    {
        // Arrange
        var classId = Guid.NewGuid();
        var studentId = Guid.NewGuid();

        _classRepositoryMock.Setup(r => r.GetByIdAsync(classId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((SchoolClass?)null);

        // Act & Assert
        await Assert.ThrowsAsync<ClassNotFoundException>(() =>
            _service.AssignAsync(classId, studentId));
    }

    [Fact]
    public async Task AssignAsync_WithNonExistingStudent_ShouldThrowException()
    {
        // Arrange
        var classId = Guid.NewGuid();
        var studentId = Guid.NewGuid();
        var schoolClass = new SchoolClass("Math", "Mr. Smith");
        SetId(schoolClass, classId);

        _classRepositoryMock.Setup(r => r.GetByIdAsync(classId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(schoolClass);
        _studentRepositoryMock.Setup(r => r.GetByIdAsync(studentId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Student?)null);

        // Act & Assert
        await Assert.ThrowsAsync<StudentNotFoundException>(() =>
            _service.AssignAsync(classId, studentId));
    }

    [Fact]
    public async Task AssignAsync_WithMaxStudents_ShouldThrowException()
    {
        // Arrange
        var classId = Guid.NewGuid();
        var studentId = Guid.NewGuid();
        var schoolClass = new SchoolClass("Math", "Mr. Smith");
        SetId(schoolClass, classId);

        // Fill class to max capacity
        for (int i = 0; i < SchoolClass.MaxStudents; i++)
        {
            schoolClass.AddStudent(Guid.NewGuid());
        }

        var student = CreateTestStudent(studentId);

        _classRepositoryMock.Setup(r => r.GetByIdAsync(classId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(schoolClass);
        _studentRepositoryMock.Setup(r => r.GetByIdAsync(studentId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(student);

        // Act & Assert
        await Assert.ThrowsAsync<MaximumStudentsReachedException>(() =>
            _service.AssignAsync(classId, studentId));
    }

    [Fact]
    public async Task AssignAsync_WithDuplicateStudent_ShouldThrowException()
    {
        // Arrange
        var classId = Guid.NewGuid();
        var studentId = Guid.NewGuid();
        var schoolClass = new SchoolClass("Math", "Mr. Smith");
        SetId(schoolClass, classId);
        schoolClass.AddStudent(studentId);
        var student = CreateTestStudent(studentId);

        _classRepositoryMock.Setup(r => r.GetByIdAsync(classId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(schoolClass);
        _studentRepositoryMock.Setup(r => r.GetByIdAsync(studentId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(student);

        // Act & Assert
        await Assert.ThrowsAsync<StudentAlreadyAssignedToClassException>(() =>
            _service.AssignAsync(classId, studentId));
    }

    [Fact]
    public async Task UnassignAsync_WithValidData_ShouldUnassignStudent()
    {
        // Arrange
        var classId = Guid.NewGuid();
        var studentId = Guid.NewGuid();
        var schoolClass = new SchoolClass("Math", "Mr. Smith");
        SetId(schoolClass, classId);
        schoolClass.AddStudent(studentId);

        _classRepositoryMock.Setup(r => r.GetByIdAsync(classId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(schoolClass);
        _classRepositoryMock.Setup(r => r.UpdateAsync(It.IsAny<SchoolClass>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _service.UnassignAsync(classId, studentId);

        // Assert
        Assert.Empty(schoolClass.StudentIds);
        _classRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<SchoolClass>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UnassignAsync_WithNonExistingClass_ShouldThrowException()
    {
        // Arrange
        var classId = Guid.NewGuid();
        var studentId = Guid.NewGuid();

        _classRepositoryMock.Setup(r => r.GetByIdAsync(classId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((SchoolClass?)null);

        // Act & Assert
        await Assert.ThrowsAsync<ClassNotFoundException>(() =>
            _service.UnassignAsync(classId, studentId));
    }

    private static void SetId(SchoolClass schoolClass, Guid id)
    {
        var idProperty = typeof(SchoolClass).GetProperty("Id", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public);
        idProperty?.SetValue(schoolClass, id);
    }

    private static Student CreateTestStudent(Guid id)
    {
        var studentId = new Domain.ValueObjects.StudentId("STU001");
        var address = new Domain.ValueObjects.Address(null, null, null);
        var student = new Student(studentId, "John", "Doe", new DateOnly(2010, 1, 1), address);
        
        var idProperty = typeof(Student).GetProperty("Id", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public);
        idProperty?.SetValue(student, id);
        
        return student;
    }
}

