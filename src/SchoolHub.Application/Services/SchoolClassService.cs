using SchoolHub.Application.DTOs;
using SchoolHub.Application.Interfaces;
using SchoolHub.Application.Mappers;
using SchoolHub.Domain.Entities;

namespace SchoolHub.Application.Services;

public sealed class SchoolClassService(ISchoolClassRepository classes, IStudentRepository students)
{
    public async Task<List<SchoolClassDto>> GetAllAsync(CancellationToken ct = default)
    {
        var sClasses = await classes.GetAllAsync(ct);
        return sClasses.Select(c => c.ToDto()).ToList();
    }

    public async Task<SchoolClassDto> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var sClass = await classes.GetByIdAsync(id, ct);
        if (sClass is null)
            throw new InvalidOperationException("Class not found");
        
        return sClass.ToDto();
    }
    public async Task<SchoolClassDto> CreateAsync(
        string name,
        string teacher,
        CancellationToken ct = default)
    {
        var sClass = new SchoolClass(name, teacher);

        await classes.AddAsync(sClass, ct);

        return sClass.ToDto();
    }
    
    public async Task<SchoolClassDto> UpdateAsync(
        Guid id,
        string name,
        string teacher,
        CancellationToken ct = default)
    {
        var sClass = await classes.GetByIdAsync(id, ct)
                     ?? throw new InvalidOperationException("Class not found");

        sClass.Update(name, teacher);

        await classes.UpdateAsync(sClass, ct);

        return sClass.ToDto();
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var sClass = await classes.GetByIdAsync(id, ct)
                     ?? throw new InvalidOperationException("Class not found");

        await classes.RemoveAsync(sClass, ct);
    }

    public async Task AssignAsync(Guid id, Guid studentId, CancellationToken ct = default)
    {
        var sClass = await classes.GetByIdAsync(id, ct);
        if (sClass is null)
            throw new InvalidOperationException("Class not found");
        
        var student = await students.GetByIdAsync(studentId, ct);
        if (student is null)
            throw new InvalidOperationException("Student not found");
        
        if (sClass.StudentIds.Contains(studentId))
            throw new InvalidOperationException("Student already assigned");
        
        sClass.AddStudent(studentId);
    }
    
    public async Task UnassignAsync(Guid classId, Guid studentId, CancellationToken ct = default)
    {
        var sClass = await classes.GetByIdAsync(classId, ct);
        if (sClass is null)
            throw new InvalidOperationException("Class not found");

        sClass.RemoveStudent(studentId);
    }
}