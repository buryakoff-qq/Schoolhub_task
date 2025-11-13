namespace SchoolHub.Application.DTOs;

public sealed record SchoolClassDto (
    Guid Id,
    string Name,
    string Teacher,
    IReadOnlyCollection<Guid> StudentIds
);