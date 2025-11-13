using SchoolHub.Domain.Entities;

namespace SchoolHub.Application.Interfaces;

public interface ISchoolClassRepository
{
    Task<SchoolClass> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<List<SchoolClass>> GetAllAsync(CancellationToken ct = default);
    Task AddAsync(SchoolClass schoolClass, CancellationToken ct = default);
    Task UpdateAsync(SchoolClass schoolClass, CancellationToken ct = default);
    Task RemoveAsync(SchoolClass schoolClass, CancellationToken ct = default);
}