using SchoolHub.Domain.Entities;

namespace SchoolHub.Application.Interfaces;

public interface IStudentRepository
{
    Task<Student> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<Student> GetByStudentIdAsync(string value, CancellationToken ct = default);
    Task<List<Student>> GetAllAsync(CancellationToken ct = default);
    Task AddAsync(Student student, CancellationToken ct = default);
    Task UpdateAsync(Student student, CancellationToken ct = default);
    Task RemoveAsync(Student student, CancellationToken ct = default);
}