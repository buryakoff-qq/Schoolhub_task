using SchoolHub.Application.DTOs;
using SchoolHub.Domain.Entities;

namespace SchoolHub.Application.Mappers;

public static class DtoMapper
{
    public static StudentDto ToDto(this Student s)
        => new(
            s.Id,
            s.StudentId.Value,
            s.FirstName,
            s.LastName,
            s.BirthDate,
            s.Address.City,
            s.Address.Street,
            s.Address.PostalCode);

    public static SchoolClassDto ToDto(this SchoolClass c)
        => new(
            c.Id,
            c.Name,
            c.Teacher,
            c.StudentIds);
}