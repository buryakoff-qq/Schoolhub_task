using SchoolHub.Application.DTOs;
using SchoolHub.Application.Exceptions;
using SchoolHub.Application.Interfaces;
using SchoolHub.Application.Mappers;
using SchoolHub.Domain.Entities;
using SchoolHub.Domain.ValueObjects;

namespace SchoolHub.Application.Services;

public sealed class StudentService(IStudentRepository studentRepository)
{
    public async Task<List<StudentDto>> GetAllAsync(CancellationToken ct = default)
    {
        var students = await studentRepository.GetAllAsync(ct);
        return students.Select(s => s.ToDto()).ToList();
    }

    public async Task<StudentDto> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var student = await studentRepository.GetByIdAsync(id, ct);
        if (student is null)
            throw new StudentNotFoundException();
        return student.ToDto();
    }
    public async Task<StudentDto> GetByStudentIdAsync(string value, CancellationToken ct = default)
    {
        var student = await studentRepository.GetByStudentIdAsync(value, ct)
                      ?? throw new StudentNotFoundException();

        return student.ToDto();
    }

    public async Task<StudentDto> CreateAsync(string studentId, string firstName, string lastName, DateOnly birthDate,
        string? city, string? street, string? postalCode, CancellationToken ct = default)
    {
        var exists = await studentRepository.GetByStudentIdAsync(studentId, ct);
        if (exists is not null)
            throw new StudentIdMustBeUniqueException();

        var sid = new StudentId(studentId);
        var address = new Address(city, street, postalCode);

        var student = new Student(sid, firstName, lastName, birthDate, address);

        await studentRepository.AddAsync(student, ct);
        
        return student.ToDto();
    }

    public async Task<StudentDto> UpdateAsync(Guid id, string studentId, string firstName, string lastName,
        DateOnly birthDate, string? city, string? street, string? postalCode, CancellationToken ct = default)
    {
        var student = await studentRepository.GetByIdAsync(id, ct);
        if (student is null)
            throw new StudentNotFoundException();

        var existingStudent = await studentRepository.GetByStudentIdAsync(studentId, ct);
        if (existingStudent is not null && existingStudent.Id != id)
            throw new StudentIdMustBeUniqueException();
        
        student.Update(
            new StudentId(studentId),
            firstName,
            lastName,
            birthDate,
            new Address(city, street, postalCode)
            );

        await studentRepository.UpdateAsync(student, ct);

        return student.ToDto();
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var student = await studentRepository.GetByIdAsync(id, ct)
                      ?? throw new StudentNotFoundException();

        await studentRepository.RemoveAsync(student, ct);
    }
}