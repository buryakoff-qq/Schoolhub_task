using Moq;
using SchoolHub.Application.DTOs;
using SchoolHub.Application.Exceptions;
using SchoolHub.Application.Interfaces;
using SchoolHub.Application.Services;
using SchoolHub.Domain.Entities;
using SchoolHub.Domain.ValueObjects;

namespace SchoolHub.Application.Tests.Services;

public class StudentServiceTests
{
    private readonly Mock<IStudentRepository> _repositoryMock;
    private readonly StudentService _service;

    public StudentServiceTests()
    {
        _repositoryMock = new Mock<IStudentRepository>();
        _service = new StudentService(_repositoryMock.Object);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllStudents()
    {
        // Arrange
        var students = new List<Student>
        {
            CreateTestStudent(Guid.NewGuid(), "STU001", "John", "Doe"),
            CreateTestStudent(Guid.NewGuid(), "STU002", "Jane", "Smith")
        };

        _repositoryMock.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(students);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal("STU001", result[0].StudentId);
        Assert.Equal("STU002", result[1].StudentId);
    }

    [Fact]
    public async Task GetByIdAsync_WithExistingId_ShouldReturnStudent()
    {
        // Arrange
        var id = Guid.NewGuid();
        var student = CreateTestStudent(id, "STU001", "John", "Doe");

        _repositoryMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(student);

        // Act
        var result = await _service.GetByIdAsync(id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(id, result.Id);
        Assert.Equal("STU001", result.StudentId);
    }

    [Fact]
    public async Task GetByIdAsync_WithNonExistingId_ShouldThrowException()
    {
        // Arrange
        var id = Guid.NewGuid();
        _repositoryMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Student?)null);

        // Act & Assert
        await Assert.ThrowsAsync<StudentNotFoundException>(() =>
            _service.GetByIdAsync(id));
    }

    [Fact]
    public async Task GetByStudentIdAsync_WithExistingStudentId_ShouldReturnStudent()
    {
        // Arrange
        var student = CreateTestStudent(Guid.NewGuid(), "STU001", "John", "Doe");

        _repositoryMock.Setup(r => r.GetByStudentIdAsync("STU001", It.IsAny<CancellationToken>()))
            .ReturnsAsync(student);

        // Act
        var result = await _service.GetByStudentIdAsync("STU001");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("STU001", result.StudentId);
    }

    [Fact]
    public async Task GetByStudentIdAsync_WithNonExistingStudentId_ShouldThrowException()
    {
        // Arrange
        _repositoryMock.Setup(r => r.GetByStudentIdAsync("STU999", It.IsAny<CancellationToken>()))
            .ReturnsAsync((Student?)null);

        // Act & Assert
        await Assert.ThrowsAsync<StudentNotFoundException>(() =>
            _service.GetByStudentIdAsync("STU999"));
    }

    [Fact]
    public async Task CreateAsync_WithValidData_ShouldCreateStudent()
    {
        // Arrange
        _repositoryMock.Setup(r => r.GetByStudentIdAsync("STU001", It.IsAny<CancellationToken>()))
            .ReturnsAsync((Student?)null);
        _repositoryMock.Setup(r => r.AddAsync(It.IsAny<Student>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _service.CreateAsync(
            "STU001", "John", "Doe", new DateOnly(2010, 1, 1),
            "City", "Street", "12345");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("STU001", result.StudentId);
        Assert.Equal("John", result.FirstName);
        Assert.Equal("Doe", result.LastName);
        _repositoryMock.Verify(r => r.AddAsync(It.IsAny<Student>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_WithDuplicateStudentId_ShouldThrowException()
    {
        // Arrange
        var existingStudent = CreateTestStudent(Guid.NewGuid(), "STU001", "John", "Doe");
        _repositoryMock.Setup(r => r.GetByStudentIdAsync("STU001", It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingStudent);

        // Act & Assert
        await Assert.ThrowsAsync<StudentIdMustBeUniqueException>(() =>
            _service.CreateAsync("STU001", "Jane", "Smith", new DateOnly(2010, 1, 1),
                null, null, null));
    }

    [Fact]
    public async Task UpdateAsync_WithValidData_ShouldUpdateStudent()
    {
        // Arrange
        var id = Guid.NewGuid();
        var student = CreateTestStudent(id, "STU001", "John", "Doe");

        _repositoryMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(student);
        _repositoryMock.Setup(r => r.GetByStudentIdAsync("STU002", It.IsAny<CancellationToken>()))
            .ReturnsAsync((Student?)null);
        _repositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Student>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _service.UpdateAsync(
            id, "STU002", "Jane", "Smith", new DateOnly(2011, 1, 1),
            "NewCity", "NewStreet", "99999");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("STU002", result.StudentId);
        Assert.Equal("Jane", result.FirstName);
        _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Student>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_WithNonExistingId_ShouldThrowException()
    {
        // Arrange
        var id = Guid.NewGuid();
        _repositoryMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Student?)null);

        // Act & Assert
        await Assert.ThrowsAsync<StudentNotFoundException>(() =>
            _service.UpdateAsync(id, "STU001", "John", "Doe", new DateOnly(2010, 1, 1),
                null, null, null));
    }

    [Fact]
    public async Task UpdateAsync_WithDuplicateStudentId_ShouldThrowException()
    {
        // Arrange
        var id = Guid.NewGuid();
        var student = CreateTestStudent(id, "STU001", "John", "Doe");
        var otherStudent = CreateTestStudent(Guid.NewGuid(), "STU002", "Jane", "Smith");

        _repositoryMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(student);
        _repositoryMock.Setup(r => r.GetByStudentIdAsync("STU002", It.IsAny<CancellationToken>()))
            .ReturnsAsync(otherStudent);

        // Act & Assert
        await Assert.ThrowsAsync<StudentIdMustBeUniqueException>(() =>
            _service.UpdateAsync(id, "STU002", "John", "Doe", new DateOnly(2010, 1, 1),
                null, null, null));
    }

    [Fact]
    public async Task UpdateAsync_WithSameStudentId_ShouldNotThrowException()
    {
        // Arrange
        var id = Guid.NewGuid();
        var student = CreateTestStudent(id, "STU001", "John", "Doe");

        _repositoryMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(student);
        _repositoryMock.Setup(r => r.GetByStudentIdAsync("STU001", It.IsAny<CancellationToken>()))
            .ReturnsAsync(student);
        _repositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Student>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _service.UpdateAsync(
            id, "STU001", "Jane", "Smith", new DateOnly(2010, 1, 1),
            null, null, null);

        // Assert
        Assert.NotNull(result);
        _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Student>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_WithExistingId_ShouldDeleteStudent()
    {
        // Arrange
        var id = Guid.NewGuid();
        var student = CreateTestStudent(id, "STU001", "John", "Doe");

        _repositoryMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(student);
        _repositoryMock.Setup(r => r.RemoveAsync(It.IsAny<Student>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _service.DeleteAsync(id);

        // Assert
        _repositoryMock.Verify(r => r.RemoveAsync(It.IsAny<Student>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_WithNonExistingId_ShouldThrowException()
    {
        // Arrange
        var id = Guid.NewGuid();
        _repositoryMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Student?)null);

        // Act & Assert
        await Assert.ThrowsAsync<StudentNotFoundException>(() =>
            _service.DeleteAsync(id));
    }

    private static Student CreateTestStudent(Guid id, string studentId, string firstName, string lastName)
    {
        var sid = new StudentId(studentId);
        var address = new Address(null, null, null);
        var student = new Student(sid, firstName, lastName, new DateOnly(2010, 1, 1), address);
        
        // Use reflection to set Id for testing
        var idProperty = typeof(Student).GetProperty("Id", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public);
        idProperty?.SetValue(student, id);
        
        return student;
    }
}

