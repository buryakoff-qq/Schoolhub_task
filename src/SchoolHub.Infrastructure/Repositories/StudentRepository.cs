using Microsoft.EntityFrameworkCore;
using SchoolHub.Application.Interfaces;
using SchoolHub.Domain.Entities;
using Schoolhub.Infrastructure.Persistence;

namespace Schoolhub.Infrastructure.Repositories;

public class StudentRepository(AppDbContext db) : IStudentRepository
{
    private readonly AppDbContext _db = db;

    public Task<Student> GetByIdAsync(Guid id, CancellationToken ct = default)
        => _db.Students.FirstOrDefaultAsync(s => s.Id == id, ct);

    public Task<Student> GetByStudentIdAsync(string value, CancellationToken ct = default)
        => _db.Students.FirstOrDefaultAsync(s => s.StudentId.Value == value, ct);

    public Task<List<Student>> GetAllAsync(CancellationToken ct = default)
        => _db.Students.ToListAsync(ct);

    public Task AddAsync(Student student, CancellationToken ct = default)
        => _db.Students.AddAsync(student, ct).AsTask();

    public Task UpdateAsync(Student student, CancellationToken ct = default)
    {
        _db.Students.Update(student);
        return Task.CompletedTask;
    }

    public Task RemoveAsync(Student student, CancellationToken ct = default)
    {
        _db.Students.Remove(student);
        return Task.CompletedTask;
    }

    public Task SaveChangesAsync(CancellationToken ct = default)
        => _db.SaveChangesAsync(ct);
}