using SchoolHub.Application.DTOs;
using SchoolHub.Application.Services;

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
            return Results.Ok(await service.GetByIdAsync(id, ct));
        });

        endpoints.MapPost("/classes", async (
            SchoolClassCreateRequest req,
            SchoolClassService service,
            CancellationToken ct) =>
        {
            var result = await service.CreateAsync(req.Name, req.Teacher, ct);
            return Results.Created($"/classes/{result.Id}", result);
        });

        endpoints.MapPut("/classes/{id:guid}", async (
            Guid id,
            SchoolClassUpdateRequest req,
            SchoolClassService service,
            CancellationToken ct) =>
        {
            var result = await service.UpdateAsync(id, req.Name, req.Teacher, ct);
            return Results.Ok(result);
        });

        endpoints.MapDelete("/classes/{id:guid}", async (
            Guid id,
            SchoolClassService service,
            CancellationToken ct) =>
        {
            await service.DeleteAsync(id, ct);
            return Results.NoContent();
        });

        endpoints.MapPost("/classes/{classId:guid}/assign/{studentId:guid}", async (
            Guid classId,
            Guid studentId,
            SchoolClassService service,
            CancellationToken ct) =>
        {
            await service.AssignAsync(classId, studentId, ct);
            return Results.Ok();
        });

        endpoints.MapDelete("/classes/{classId:guid}/unassign/{studentId:guid}", async (
            Guid classId,
            Guid studentId,
            SchoolClassService service,
            CancellationToken ct) =>
        {
            await service.UnassignAsync(classId, studentId, ct);
            return Results.NoContent();
        });
        
        return endpoints;
    }
}