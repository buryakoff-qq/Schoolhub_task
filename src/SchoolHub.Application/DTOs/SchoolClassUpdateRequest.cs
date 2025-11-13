namespace SchoolHub.Application.DTOs;

public sealed record SchoolClassUpdateRequest(
    string Name,
    string Teacher
);