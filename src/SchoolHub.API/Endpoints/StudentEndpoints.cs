using SchoolHub.Application.DTOs;
using SchoolHub.Application.Exceptions;
using SchoolHub.Application.Services;
using SchoolHub.Domain.Exceptions;

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
            try
            {
                return Results.Ok(await service.GetByIdAsync(id, ct));
            }
            catch (StudentNotFoundException)
            {
                return Results.NotFound();
            }
        });

        endpoints.MapGet("/students/by-id/{studentId}", async (
            string studentId,
            StudentService service,
            CancellationToken ct) =>
        {
            try
            {
                var student = await service.GetByStudentIdAsync(studentId, ct);
                return Results.Ok(student);
            }
            catch (StudentNotFoundException)
            {
                return Results.NotFound();
            }
        });

        endpoints.MapPost("/students", async (
            StudentCreateRequest req,
            StudentService service,
            CancellationToken ct) =>
        {
            try
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
            }
            catch (StudentIdMustBeUniqueException)
            {
                return Results.BadRequest();
            }
            catch (StudentDomainException)
            {
                return Results.BadRequest();
            }
        });

        endpoints.MapPut("/students/{id:guid}", async (
            Guid id,
            StudentUpdateRequest req,
            StudentService service,
            CancellationToken ct) =>
        {
            try
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
            }
            catch (StudentNotFoundException)
            {
                return Results.NotFound();
            }
            catch (StudentIdMustBeUniqueException)
            {
                return Results.BadRequest();
            }
            catch (StudentDomainException)
            {
                return Results.BadRequest();
            }
        });

        endpoints.MapDelete("/students/{id:guid}", async (
            Guid id,
            StudentService service,
            CancellationToken ct) =>
        {
            try
            {
                await service.DeleteAsync(id, ct);
                return Results.NoContent();
            }
            catch (StudentNotFoundException)
            {
                return Results.NotFound();
            }
        });
        
        return endpoints;
    }
}