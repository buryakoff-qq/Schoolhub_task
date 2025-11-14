using Microsoft.EntityFrameworkCore;
using SchoolHub.Application.Interfaces;
using SchoolHub.Domain.Entities;
using SchoolHub.Infrastructure.Persistence;

namespace SchoolHub.Infrastructure.Repositories;

public sealed class StudentRepository(AppDbContext db) : IStudentRepository
{
    public async Task<Student?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await db.Students.FirstOrDefaultAsync(s => s.Id == id, ct);
    }

    public async Task<Student?> GetByStudentIdAsync(string value, CancellationToken ct = default)
    {
        return await db.Students.FirstOrDefaultAsync(s => s.StudentId.Value == value, ct);
    }

    public async Task<List<Student>> GetAllAsync(CancellationToken ct = default)
    {
        return await db.Students.ToListAsync(ct);
    }

    public async Task AddAsync(Student student, CancellationToken ct = default)
    {
        await db.Students.AddAsync(student, ct);
        await db.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(Student student, CancellationToken ct = default)
    {
        db.Students.Update(student);
        await db.SaveChangesAsync(ct);
    }

    public async Task RemoveAsync(Student student, CancellationToken ct = default)
    {
        db.Students.Remove(student);
        await db.SaveChangesAsync(ct);
    }
}