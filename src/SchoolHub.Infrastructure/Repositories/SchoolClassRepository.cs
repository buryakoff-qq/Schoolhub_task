using Microsoft.EntityFrameworkCore;
using SchoolHub.Application.Interfaces;
using SchoolHub.Domain.Entities;
using SchoolHub.Infrastructure.Persistence;

namespace SchoolHub.Infrastructure.Repositories;

public sealed class SchoolClassRepository(AppDbContext db) : ISchoolClassRepository
{
    public async Task<SchoolClass> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await db.SchoolClasses.FirstOrDefaultAsync(c => c.Id == id, ct);
    }

    public async Task<List<SchoolClass>> GetAllAsync(CancellationToken ct = default)
    {
        return await db.SchoolClasses.ToListAsync(ct);
    }

    public async Task AddAsync(SchoolClass sClass, CancellationToken ct = default)
    {
        await db.SchoolClasses.AddAsync(sClass, ct);  
        await db.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(SchoolClass sClass, CancellationToken ct = default)
    {
        db.SchoolClasses.Update(sClass);
        await db.SaveChangesAsync(ct);
    }

    public async Task RemoveAsync(SchoolClass sClass, CancellationToken ct = default)
    {
        db.SchoolClasses.Remove(sClass);
        await db.SaveChangesAsync(ct);
    }
}