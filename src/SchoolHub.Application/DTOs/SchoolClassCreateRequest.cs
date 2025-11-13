namespace SchoolHub.Application.DTOs;

public sealed record SchoolClassCreateRequest(
    string Name,
    string Teacher
);