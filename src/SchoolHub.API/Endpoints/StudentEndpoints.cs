using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc.Filters;
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
            try
            {
                return Results.Ok(await service.GetByIdAsync(id, ct));
            }
            catch (InvalidOperationException e)
            {
                return Results.NotFound(e.Message);
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
            catch (InvalidOperationException e)
            {
                return Results.NotFound(e.Message);
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
            catch (InvalidOperationException e)
            {
                return e.Message == "Student not found" ? Results.NotFound(e.Message) : Results.BadRequest(e.Message);
            }
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
            try
            {
                await service.DeleteAsync(id, ct);
                return Results.NoContent();
            }
            catch (InvalidOperationException e)
            {
                return Results.NotFound(e.Message);
            }
        });
        
        return endpoints;
    }
}