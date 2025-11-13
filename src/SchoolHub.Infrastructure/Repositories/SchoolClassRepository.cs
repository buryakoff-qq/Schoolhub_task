using Microsoft.EntityFrameworkCore;
using SchoolHub.Application.Interfaces;
using SchoolHub.Domain.Entities;
using Schoolhub.Infrastructure.Persistence;

namespace Schoolhub.Infrastructure.Repositories;

public sealed class SchoolClassRepository(AppDbContext db) : ISchoolClassRepository
{
    private readonly AppDbContext _db = db;

    public Task<SchoolClass?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => _db.SchoolClasses.FirstOrDefaultAsync(s => s.Id == id, ct);

    public Task<List<SchoolClass>> GetAllAsync(CancellationToken ct = default)
        => _db.SchoolClasses.ToListAsync(ct);

    public Task AddAsync(SchoolClass sClass, CancellationToken ct = default)
        => _db.SchoolClasses.AddAsync(sClass, ct).AsTask();

    public Task UpdateAsync(SchoolClass sClass, CancellationToken ct = default)
    {
        _db.SchoolClasses.Update(sClass);
        return Task.CompletedTask;
    }

    public Task RemoveAsync(SchoolClass sClass, CancellationToken ct = default)
    {
        _db.SchoolClasses.Remove(sClass);
        return Task.CompletedTask;
    }

    public Task SaveChangesAsync(CancellationToken ct = default)
        => _db.SaveChangesAsync(ct);
}