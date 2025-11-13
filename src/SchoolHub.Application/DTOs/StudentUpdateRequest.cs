namespace SchoolHub.Application.DTOs;

public sealed record StudentUpdateRequest(
    string StudentId,
    string FirstName,
    string LastName,
    DateOnly BirthDate,
    string? City,
    string? Street,
    string? PostalCode
);