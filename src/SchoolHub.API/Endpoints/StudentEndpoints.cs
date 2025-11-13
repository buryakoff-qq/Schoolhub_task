using SchoolHub.Application.DTOs;
using SchoolHub.Application.Services;

namespace SchoolHub.API.Endpoints;

public static class StudentEndpoints
{
    public static IEndpointRouteBuilder MapStudentEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/students", async (StudentService service, CancellationToken ct) =>
        {
            return Results.Ok(await service.GetAllAsync(ct));
        });

        endpoints.MapGet("/students/{id:guid}", async (Guid id, StudentService service, CancellationToken ct) =>
        {
            return Results.Ok(await service.GetByIdAsync(id, ct));
        });

        endpoints.MapGet("/students/by-id/{studentId}", async (
            string studentId,
            StudentService service,
            CancellationToken ct) =>
        {
            var student = await service.GetByStudentIdAsync(studentId, ct);
            return Results.Ok(student);
        });

        endpoints.MapPost("/students", async (
            StudentCreateRequest req,
            StudentService service,
            CancellationToken ct) =>
        {
            var result = await service.CreateAsync(
                req.StudentId,
                req.FirstName,
                req.LastName,
                req.BirthDate,
                req.City,
                req.Street,
                req.PostalCode,
                ct);

            return Results.Created($"/students/{result.Id}", result);
        });

        endpoints.MapPut("/students/{id:guid}", async (
            Guid id,
            StudentUpdateRequest req,
            StudentService service,
            CancellationToken ct) =>
        {
            var result = await service.UpdateAsync(
                id,
                req.StudentId,
                req.FirstName,
                req.LastName,
                req.BirthDate,
                req.City,
                req.Street,
                req.PostalCode,
                ct);

            return Results.Ok(result);
        });

        endpoints.MapDelete("/students/{id:guid}", async (
            Guid id,
            StudentService service,
            CancellationToken ct) =>
        {
            await service.DeleteAsync(id, ct);
            return Results.NoContent();
        });
        
        return endpoints;
    }
}