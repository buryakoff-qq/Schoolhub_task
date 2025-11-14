using SchoolHub.Application.DTOs;
using SchoolHub.Application.Exceptions;
using SchoolHub.Application.Services;
using SchoolHub.Domain.Exceptions;

namespace SchoolHub.API.Endpoints;

public static class ClassesEndpoints
{
    public static IEndpointRouteBuilder MapClassesEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/classes", async (SchoolClassService service, CancellationToken ct) =>
        {
            return Results.Ok(await service.GetAllAsync(ct));
        });

        endpoints.MapGet("/classes/{id:guid}", async (Guid id, SchoolClassService service, CancellationToken ct) =>
        {
            try
            {
                return Results.Ok(await service.GetByIdAsync(id, ct));
            }
            catch (ClassNotFoundException)
            {
                return Results.NotFound();
            }
        });

        endpoints.MapPost("/classes", async (
            SchoolClassCreateRequest req,
            SchoolClassService service,
            CancellationToken ct) =>
        {
            try
            {
                var result = await service.CreateAsync(req.Name, req.Teacher, ct);
                return Results.Created($"/classes/{result.Id}", result);
            }
            catch (SchoolClassDomainException)
            {
                return Results.BadRequest();
            }
        });

        endpoints.MapPut("/classes/{id:guid}", async (
            Guid id,
            SchoolClassUpdateRequest req,
            SchoolClassService service,
            CancellationToken ct) =>
        {
            try
            {
                var result = await service.UpdateAsync(id, req.Name, req.Teacher, ct);
                return Results.Ok(result);
            }
            catch (ClassNotFoundException)
            {
                return Results.NotFound();
            }
            catch (SchoolClassDomainException)
            {
                return Results.BadRequest();
            }
        });

        endpoints.MapDelete("/classes/{id:guid}", async (
            Guid id,
            SchoolClassService service,
            CancellationToken ct) =>
        {
            try
            {
                await service.DeleteAsync(id, ct);
                return Results.NoContent();
            }
            catch (ClassNotFoundException)
            {
                return Results.NotFound();
            }
        });

        endpoints.MapPost("/classes/{classId:guid}/assign/{studentId:guid}", async (
            Guid classId,
            Guid studentId,
            SchoolClassService service,
            CancellationToken ct) =>
        {
            try
            {
                await service.AssignAsync(classId, studentId, ct);
                return Results.Ok();
            }
            catch (ClassNotFoundException)
            {
                return Results.NotFound();
            }
            catch (StudentNotFoundException)
            {
                return Results.NotFound();
            }
            catch (StudentAlreadyAssignedToClassException)
            {
                return Results.BadRequest();
            }
            catch (MaximumStudentsReachedException)
            {
                return Results.BadRequest();
            }
        });

        endpoints.MapDelete("/classes/{classId:guid}/unassign/{studentId:guid}", async (
            Guid classId,
            Guid studentId,
            SchoolClassService service,
            CancellationToken ct) =>
        {
            try
            {
                await service.UnassignAsync(classId, studentId, ct);
                return Results.NoContent();
            }
            catch (ClassNotFoundException)
            {
                return Results.NotFound();
            }
        });
        
        return endpoints;
    }
}